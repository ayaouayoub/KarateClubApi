using System;
using System.Data;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Domain.ValueObjs;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;
using static KarateClub.Application.Security.Permissions;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetUserByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", id);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (user is null)
                {
                    Person person = Person.Load
                    (
                        id: (int)reader["PersonId"],
                        name: (string)reader["Name"],
                        address: reader["Address"] as string,
                        email: new Email((string)reader["Email"])
                    );

                    user = User.LoadWithPerson
                    (
                        id: (int)reader["UserId"],
                        username: (string)reader["Username"],
                        passwordHash: (string)reader["PasswordHash"],
                        isSuperAdmin: (bool)reader["IsSuperAdmin"],
                        isActive: (bool)reader["IsActive"],
                        createdAt: (DateTime)reader["CreatedAt"],
                        person: person
                    );
                }

                AddPermissionIfExists(reader, user);
            }

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            User? user = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetUserByUserName", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", username);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (user is null)
                {
                    user = User.Load
                    (
                        id: (int)reader["UserId"],
                        username: (string)reader["UserName"],
                        passwordHash: (string)reader["PasswordHash"],
                        isSuperAdmin: (bool)reader["IsSuperAdmin"],
                        isActive: (bool)reader["IsActive"],
                        createdAt: (DateTime)reader["CreatedAt"],
                        personId: (int)reader["PersonId"]
                    );
                }

                AddPermissionIfExists(reader, user);
            }

            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            Dictionary<int, User> users = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetUsers", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int userId = (int)reader["UserId"];

                if (!users.TryGetValue(userId, out User? user))
                {
                    user = User.Load
                    (
                        id: userId,
                        username: (string)reader["Username"],
                        passwordHash: (string)reader["PasswordHash"],
                        isSuperAdmin: (bool)reader["IsSuperAdmin"],
                        isActive: (bool)reader["IsActive"],
                        createdAt: (DateTime)reader["CreatedAt"],
                        personId: (int)reader["PersonId"]
                    );

                    users.Add(userId, user);
                }

                AddPermissionIfExists(reader, user);
            }

            return users.Values.ToList();
        }

        private static void AddPermissionIfExists(SqlDataReader reader, User? user)
        {
            if (user is null)
                return;

            if (reader["PermissionId"] == DBNull.Value)
                return;

            user.AddPermission
            (
                Permission.Load
                (
                    id: (int)reader["PermissionId"],
                    code: (string)reader["Code"]
                )
            );
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_DeactivateUser", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", id);

            await connection.OpenAsync();

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<int> AddUserAsync(User user)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_AddNewUser", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsSuperAdmin", user.IsSuperAdmin);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
            command.Parameters.AddWithValue("@PersonId", user.PersonId);

            command.Parameters.Add(_CreatePermissionsParameter(user.Permissions));

            SqlParameter output = new("@NewUserID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(output);

            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<User?> GetUserByPersonIdAsync(int personId)
        {
            User? user = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetUserByPersonId", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonId", personId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                user = User.Load
                (
                    id: (int)reader["UserId"],
                    username: (string)reader["Username"],
                    passwordHash: (string)reader["PasswordHash"],
                    isSuperAdmin: (bool)reader["IsSuperAdmin"],
                    isActive: (bool)reader["IsActive"],
                    createdAt: (DateTime)reader["CreatedAt"],
                    personId: personId
                );
            }

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_UpdateUser", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", user.Id);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@PersonId", user.PersonId);

            command.Parameters.Add(_CreatePermissionsParameter(user.Permissions));

            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }

        private static SqlParameter _CreatePermissionsParameter(IEnumerable<Permission> permissions)
        {
            DataTable table = new();

            table.Columns.Add("PermissionId", typeof(int));

            foreach (Permission permission in permissions)
            {
                table.Rows.Add(permission.Id);
            }

            return new SqlParameter("@Permissions", SqlDbType.Structured)
            {
                TypeName = "PermissionIdTableType",
                Value = table
            };
        }
        
        public async Task<bool> ChangeMyUsernameAsync(int userId, string username)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_UpdateMyUsername", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }
    }
}
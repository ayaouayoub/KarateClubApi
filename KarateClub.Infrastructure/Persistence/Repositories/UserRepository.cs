using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Domain.ValueObjs;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;

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

            List<Permission> permissions = new();

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
                        id: (int)reader["PersonID"],
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

                if (reader["PermissionId"] != DBNull.Value)
                {
                    permissions.Add
                    (
                        Permission.Load
                        (
                            id: (int)reader["PermissionId"],
                            code: (string)reader["Code"]
                        )
                    );
                }
            }

            if (user is not null)
            {
                user.SetPermissions(permissions);
            }

            return user;
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            User? user = null;

            List<Permission> permissions = new();

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

                if (reader["PermissionId"] != DBNull.Value)
                {
                    permissions.Add(Permission.Load((int)reader["PermissionId"], (string)reader["Code"]));
                } else
                {
                    break;
                }
            }

            if (user is not null)
            {
                user.SetPermissions(permissions);
            }

            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_GetUsers", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add
                (
                    User.Load
                    (
                        id: (int)reader["UserId"],
                        username: (string)reader["Username"],
                        passwordHash: (string)reader["PasswordHash"],
                        isSuperAdmin: (bool)reader["IsSuperAdmin"],
                        isActive: (bool)reader["IsActive"],
                        createdAt: (DateTime)reader["CreatedAt"],
                        personId: (int)reader["PersonId"]
                    )
                );
            }

            return users;
        }
    }
}

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
using static KarateClub.Application.Security.Permissions;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public InstructorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<int> AddInstructorAsync(Instructor instructor)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeactivateInstructorAsync(int id)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_DeactivateIntructor", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@IntructorId", id);

            await connection.OpenAsync();

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<bool> ActivateInstructorAsync(int id)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_ActivateIntructor", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@IntructorId", id);

            await connection.OpenAsync();

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            Instructor? instructor = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetInstructorByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InstructorId", id);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                Person person = Person.Load
                (
                    id: (int)reader["PersonId"],
                    name: (string)reader["Name"],
                    address: reader["Address"] as string,
                    email: new Email((string)reader["Email"])
                );

                BeltRank beltRank = BeltRank.Load
                (
                    id: (int)reader["RankID"],
                    name: (string)reader["RankName"],
                    testFees: (decimal)reader["TestFees"]
                );

                instructor = Instructor.LoadWithPersonAndBeltRank
                (
                    id: (int)reader["InstructorID"],
                    person: person,
                    qualification: (string)reader["Qualification"],
                    isActive: (bool)reader["IsActive"],
                    beltRank: beltRank,
                    hireDate: (DateTime)reader["HireDate"]
                );
            }

            return instructor;
        }


        public async Task<Instructor?> GetByPersonIdAsync(int personId)
        {
            Instructor? instructor = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetInstructorByPersonID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonId", personId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                instructor = Instructor.Load
                (
                    id: (int)reader["InstructorID"],
                    personId: (int)reader["PersonID"],
                    qualification: (string)reader["Qualification"],
                    isActive: (bool)reader["IsActive"],
                    CurrentBeltRankID: (int)reader["CurrentBeltRankID"],
                    hireDate: (DateTime)reader["HireDate"]
                );
            }

            return instructor;
        }

        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            List<Instructor> instructors = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_GetInstructors", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                instructors.Add
                (
                    Instructor.Load
                    (
                        id: (int)reader["InstructorID"],
                        personId: (int)reader["PersonID"],
                        qualification: (string)reader["Qualification"],
                        isActive: (bool)reader["IsActive"],
                        CurrentBeltRankID: (int)reader["CurrentBeltRankID"],
                        hireDate: (DateTime)reader["HireDate"]
                    )
                );
            }

            return instructors;
        }

        public Task UpdateCurrentBletRankAsync(int instructorId, int beltRankId)
        {
            throw new NotImplementedException();
        }
    }
}

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

        public Task<bool> DeactivateInstructorAsync(int id)
        {
            throw new NotImplementedException();
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

        public Task<List<Instructor>> GetInstructorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCurrentBletRankAsync(int instructorId, int beltRankId)
        {
            throw new NotImplementedException();
        }
    }
}

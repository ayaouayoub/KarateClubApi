using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    internal class PersonRepository : IPersonRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PersonRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Person?> GetPersonById(int id)
        {
            Person? person = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetPersonByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonId", id);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                person = Person.Load
                (
                    id: (int)reader["PersonId"],
                    name: (string)reader["Name"],
                    address: (string)reader["Address"],
                    email: new Domain.ValueObjs.Email((string)reader["Email"])
                );
            }

            return person;
        }
    }
}

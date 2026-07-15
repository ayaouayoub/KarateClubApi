using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Domain.ValueObjs;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using static KarateClub.Application.Security.Permissions;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    internal class PersonRepository : IPersonRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PersonRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> AddPersonAsync(Person person)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_AddNewPerson", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonId", person.Id);
            command.Parameters.AddWithValue("@Name", person.Name);
            command.Parameters.AddWithValue("@Address", person.Address);
            command.Parameters.AddWithValue("@Email", person.Email.Value);

            SqlParameter output = new("@NewPersonId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(output);

            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            return (int)output.Value;
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            List<Person> people = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_GetPeople", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                people.Add
                (
                    Person.Load
                    (
                        id: (int)reader["PersonID"],
                        name: (string)reader["Name"],
                        address: (string)reader["Address"],
                        email: new Email((string)reader["Email"])
                    )
                );
            }

            return people;
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
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

        public async Task UpdatePersonAsync(Person person)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_UpdatePerson", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PersonId", person.Id);
            command.Parameters.AddWithValue("@Name", person.Name);
            command.Parameters.AddWithValue("@Address", person.Address);
            command.Parameters.AddWithValue("@Email", person.Email?.Value);

            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }
    }
}

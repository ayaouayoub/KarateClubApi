using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Handlers.BeltTest.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class BeltTestRepository : IBeltTestRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BeltTestRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> RegisterAsync(RegisterBeltTestCommand command)
        {
            await using var connection = _connectionFactory.CreateConnection();

            await using var sqlCommand = new SqlCommand("usp_RegisterBeltTest", connection);

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@MemberID", command.MemberId);
            sqlCommand.Parameters.AddWithValue("@RankID", command.RankId);
            sqlCommand.Parameters.AddWithValue("@TestedByInstructorID", command.TestedByInstructorId);

            await connection.OpenAsync();

            object? result = await sqlCommand.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }
    }
}

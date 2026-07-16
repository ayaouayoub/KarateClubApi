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
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class BeltRankRepository : IBeltRankRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BeltRankRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<bool> ChangeBeltTestFeesAsync(decimal NewTestFees)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BeltRank>> GetBeltsAsync()
        {
            List<BeltRank> belts = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_GetBelts", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                belts.Add
                (
                    BeltRank.Load
                    (
                        id: (int)reader["RankID"],
                        name: (string)reader["RankName"],
                        testFees: (decimal)reader["TestFees"]
                    )
                );
            }

            return belts;
        }

        public async Task<BeltRank?> GetByIdAsync(int id)
        {
            BeltRank? beltRank = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetBeltByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BeltRankID", id);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                beltRank = BeltRank.Load((int)reader["RankID"], (string)reader["RankName"], (decimal)reader["TestFees"]);
            }

            return beltRank;
        }
    }
}

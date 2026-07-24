using System.Data;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Domain.ValueObjs;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class SubscriptionPeriodRepository : ISubscriptionPeriodRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SubscriptionPeriodRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<SubscriptionPeriod?> GetCurrentPeriodAsync(int memberId)
        {
            SubscriptionPeriod? subscriptionPeriod = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetCurrentPeriod", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MemberID", memberId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                subscriptionPeriod = SubscriptionPeriod.Load
                (
                    (int)reader["PeriodID"], 
                    new Period((DateTime)reader["StartDate"], (DateTime)reader["EndDate"]),
                    (decimal)reader["TotalFees"],
                    (int)reader["MemberID"],
                    (int)reader["SubscriptionPlanId"]

                );
            }

            return subscriptionPeriod;
        }
    }
}

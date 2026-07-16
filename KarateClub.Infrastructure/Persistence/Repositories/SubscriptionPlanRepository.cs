using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SubscriptionPlanRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<int> AddSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivatePlanAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SubscriptionPlan?> GetByIdAsync(int id)
        {
            SubscriptionPlan? plan = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetSubscriptionPlanByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SubscriptionPlanID", id);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                plan = SubscriptionPlan.Load((int)reader["PlanID"], (string)reader["PlanName"], (int)reader["DurationInMonths"], (decimal)reader["Fees"], (bool)reader["IsActive"]);
            }

            return plan;
        }

        public Task<bool> GetPlanByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubscriptionPlan>> GetSubscriptionPlansAsync()
        {
            throw new NotImplementedException();
        }
    }
}

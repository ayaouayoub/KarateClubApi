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
    public class MemberRepository : IMemberRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MemberRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<bool> ActivateMemberAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeactivateMemberAsync(int id)
        {
            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_DeactivateMember", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@MemberId", id);

            await connection.OpenAsync();

            int affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            Member? member = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetMemberByID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MemberId", id);

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

                member = Member.LoadWithPersonAndBeltRank
                (
                    id: (int)reader["MemberID"],
                    person: person,
                    emergencyContactInfo: (string)reader["EmergencyContactInfo"],
                    isActive: (bool)reader["IsActive"],
                    beltRank: beltRank
                );
            }

            return member;
        }

        public async Task<Member?> GetByPersonIdAsync(int personId)
        {
            Member? member = null;

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetMemberByPersonID", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PersonId", personId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                member = Member.Load
                (
                    id: (int)reader["MemberID"],
                    personId: (int)reader["PersonID"],
                    emergencyContactInfo: (string)reader["EmergencyContactInfo"],
                    isActive: (bool)reader["IsActive"],
                    lastBeltRankID: (int)reader["LastBeltRank"]
                );
            }

            return member;
        }

        public async Task<List<Member>> GetMembersAsync()
        {
            List<Member> members = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();

            using SqlCommand command = new("usp_GetMembers", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                members.Add
                (
                    Member.Load
                    (
                        id: (int)reader["MemberID"],
                        personId: (int)reader["PersonID"],
                        emergencyContactInfo: (string)reader["EmergencyContactInfo"],
                        isActive: (bool)reader["IsActive"],
                        lastBeltRankID: (int)reader["lastBeltRank"]
                    )
                );
            }

            return members;
        }

        public Task<bool> UpdateCurrentBletRankAsync(int memberId, int beltRankId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmergencyContactInfoAsync(int memberId, string emergencyContactInfo)
        {
            throw new NotImplementedException();
        }
    }
}

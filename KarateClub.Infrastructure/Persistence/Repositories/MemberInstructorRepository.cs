using System.Data;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;
using KarateClub.Domain.ValueObjs;
using KarateClub.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;
using static KarateClub.Application.Security.Permissions;

namespace KarateClub.Infrastructure.Persistence.Repositories
{
    public class MemberInstructorRepository : IMemberInstructorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MemberInstructorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<MemberInstructor>> GetByInstructorIdAsync(int instructorId)
        {
            List<MemberInstructor> memberInstructor = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetByInstructorId", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InstructorId", instructorId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Person person = Person.Load
                (
                    id: (int)reader["MemberPersonId"],
                    name: (string)reader["MemberName"],
                    address: reader["MemberAddress"] as string,
                    email: new Email((string)reader["MemberEmail"])
                );

                Member member = Member.LoadWithPerson
                (
                    id: (int)reader["MemberID"],
                    person: person,
                    emergencyContactInfo: (string)reader["EmergencyContactInfo"],
                    isActive: (bool)reader["IsActive"],
                    beltRankId: (int)reader["CurrentBeltRankID"]
                );

                memberInstructor.Add
                (
                    MemberInstructor.LoadWithMember
                    (
                        instructorId: (int)reader["InstructorID"],
                        member: member,
                        assignDate: (DateTime)reader["AssignDate"]
                    )
                );
            }

            return memberInstructor;
        }

        public Task<IReadOnlyList<MemberInstructor>> GetByMemberIdAsync(int memberId)
        {
            throw new NotImplementedException();
        }
    }
}

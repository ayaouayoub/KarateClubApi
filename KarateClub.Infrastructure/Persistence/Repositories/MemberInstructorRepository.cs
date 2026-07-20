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
                    isActive: (bool)reader["IsActiveMember"],
                    beltRankId: (int)reader["LastBeltRank"]
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

        public async Task<IReadOnlyList<MemberInstructor>> GetByMemberIdAsync(int memberId)
        {
            List<MemberInstructor> memberInstructor = new();

            using SqlConnection connection = _connectionFactory.CreateConnection();
            using SqlCommand command = new("usp_GetByMemberId", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MemberId", memberId);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Person person = Person.Load
                (
                    id: (int)reader["InstructorPersonId"],
                    name: (string)reader["InstructorName"],
                    address: reader["InstructorAddress"] as string,
                    email: new Email((string)reader["InstructorEmail"])
                );

                Instructor instructor = Instructor.LoadWithPerson
                (
                    id: (int)reader["InstructorID"],
                    person: person,
                    qualification: (string)reader["Qualification"],
                    isActive: (bool)reader["IsActiveInstructor"],
                    hireDate: (DateTime)reader["HireDate"],
                    beltRankId: (int)reader["CurrentBeltRankID"]
                );

                memberInstructor.Add
                (
                    MemberInstructor.LoadWithInstructor
                    (
                        memberId: (int)reader["MemberID"],
                        instructor: instructor,
                        assignDate: (DateTime)reader["AssignDate"]
                    )
                );
            }

            return memberInstructor;
        }
    }
}

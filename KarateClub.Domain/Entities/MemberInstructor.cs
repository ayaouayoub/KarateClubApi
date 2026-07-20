using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Domain.Entities
{
    public class MemberInstructor
    {
        public Member? Member { get; private set; } = null!;
        public int MemberId { get; private set; }
        public Instructor? Instructor { get; private set; } = null!;
        public int InstructorId { get; private set; }
        public DateTime AssignDate { get; private set; }

        private MemberInstructor(Member? member, int memberId, Instructor? instructor, int instructorId, DateTime? assignDate = null)
        {
            Member = member;
            MemberId = memberId;
            Instructor = instructor;
            InstructorId = instructorId;
            AssignDate = assignDate ?? DateTime.UtcNow;
        }

        public static MemberInstructor Create(Member member, Instructor instructor)
        {
            return new MemberInstructor(member, member.Id, instructor, instructor.Id);
        }

        public static MemberInstructor Load(int memberId, int instructorId, DateTime assignDate)
        {
            return new MemberInstructor(null, memberId, null, instructorId, assignDate);
        }

        public static MemberInstructor LoadWithInstructor(int memberId, Instructor instructor, DateTime assignDate)
        {
            return new MemberInstructor(null, memberId, instructor, instructor.Id, assignDate);
        }

        public static MemberInstructor LoadWithMember(Member member, int instructorId, DateTime assignDate)
        {
            return new MemberInstructor(member, member.Id, null, instructorId, assignDate);
        }
    }
}

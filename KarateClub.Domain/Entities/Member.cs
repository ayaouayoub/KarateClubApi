using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public class Member
    {
        public int Id { get; }
        public int PersonId { get; }
        public Person? Person { get; }
        public string? EmergencyContactInfo { get; private set;  }
        public bool IsActive { get; private set; }
        public int LastBeltRankID { get; private set; }
        public BeltRank? BeltRank { get; private set; }

        private Member(int id, int personId, Person? person, string? emergencyContactInfo, bool isActive, int lastBeltRankID, BeltRank? beltRank)
        {
            if (beltRank is not null && beltRank.Id != lastBeltRankID) throw new DomainException("Belt rank id mismatch.");
            if (person is not null && person.Id != personId) throw new DomainException("Person id mismatch.");

            Id = id;
            PersonId = personId;
            Person = person;
            EmergencyContactInfo = emergencyContactInfo;
            IsActive = isActive;
            LastBeltRankID = lastBeltRankID;
            BeltRank = beltRank;
        }

        public static Member Create(Person person, BeltRank beltRank, string? emergencyContactInfo = null)
        {
            return new Member(0, person.Id, person, emergencyContactInfo, true, beltRank.Id, beltRank);
        }

        public static Member LoadWithPersonAndBeltRank(int id, Person person, string? emergencyContactInfo, bool isActive, BeltRank beltRank)
        {
            return new Member(id, person.Id, person, emergencyContactInfo, isActive, beltRank.Id, beltRank);
        }

        public static Member Load(int id, int personId, string emergencyContactInfo, bool isActive, int lastBeltRankID)
        {
            return new Member(id, personId, null, emergencyContactInfo, isActive, lastBeltRankID, null);
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("Member is already active.");

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("Member is already inactive.");

            IsActive = false;
        }

        public void ChangeBeltRank(BeltRank beltRank)
        {
            _ValdateBeltRank(beltRank);
            BeltRank = beltRank;
            LastBeltRankID = beltRank.Id;
        }

        public void ChangeEmergencyContactInfo(string emergencyContactInfo)
        {
            emergencyContactInfo = emergencyContactInfo.Trim();
            if (EmergencyContactInfo == emergencyContactInfo) throw new DomainException("Emergency contact information must be different from the current value.");
            EmergencyContactInfo = emergencyContactInfo;
        }

        private void _ValdateBeltRank(BeltRank beltRank)
        {
            if (BeltRank == null) throw new DomainException("Belt rank cannot be null");
            if (beltRank.Id == LastBeltRankID) throw new DomainException("The member already has this belt rank");
        }
    }
}

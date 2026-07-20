using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public class Instructor
    {
        public int Id { get; }
        public int PersonId { get; }
        public Person? Person { get; }
        public string Qualification { get; } = null!;
        public DateTime HireDate { get; }
        public bool IsActive { get; private set; }
        public int CurrentBeltRankID { get; private set; }
        public BeltRank? BeltRank { get; private set; }

        private Instructor(int id, int personId, Person? person, string qualification, bool isActive, int currentBeltRankID, BeltRank? beltRank, DateTime? hireDate = null)
        {
            if (beltRank is not null && beltRank.Id != currentBeltRankID) throw new DomainException("Belt rank id mismatch.");
            if (person is not null && person.Id != personId) throw new DomainException("Person id mismatch.");
            if (string.IsNullOrEmpty(qualification)) throw new DomainException("Qualification cannot be null or empty");

            Id = id;
            PersonId = personId;
            Person = person;
            Qualification = qualification;
            IsActive = isActive;
            CurrentBeltRankID = currentBeltRankID;
            BeltRank = beltRank;
            HireDate = hireDate ?? DateTime.UtcNow;
        }

        public static Instructor Create(Person person, string qualification, BeltRank beltRank)
        {
            return new Instructor(0, person.Id, person, qualification, true, beltRank.Id, beltRank);
        }

        public static Instructor LoadWithPersonAndBeltRank(int id, Person person, string qualification, bool isActive, BeltRank beltRank, DateTime hireDate)
        {
            return new Instructor(id, person.Id, person, qualification, isActive, beltRank.Id, beltRank, hireDate);
        }

        public static Instructor LoadWithPerson(int id, Person person, string qualification, bool isActive, int beltRankId, DateTime hireDate)
        {
            return new Instructor(id, person.Id, person, qualification, isActive, beltRankId, null, hireDate);
        }

        public static Instructor Load(int id, int personId, string qualification, bool isActive, int CurrentBeltRankID, DateTime hireDate)
        {
            return new Instructor(id, personId, null, qualification, isActive, CurrentBeltRankID, null, hireDate);
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("Instructor is already active.");

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("Instructor is already inactive.");

            IsActive = false;
        }

        public void ChangeBeltRank(BeltRank beltRank)
        {
            _ValdateBeltRank(beltRank);
            BeltRank = beltRank;
            CurrentBeltRankID = beltRank.Id;
        }

        private void _ValdateBeltRank(BeltRank beltRank)
        {
            if (BeltRank == null) throw new DomainException("Belt rank cannot be null");
            if (beltRank.Id == CurrentBeltRankID) throw new DomainException("The instructor already has this belt rank");
        }
    }
}

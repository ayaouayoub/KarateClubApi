using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public class BeltRank
    {
        public int Id { get; }
        public string Name { get; } = null!;
        public decimal TestFees { get; private set; }

        private BeltRank(int id, string name, decimal testFees)
        {
            _ValidateName(name);
            _ValidateFees(testFees);
            Id = id;
            Name = name;
            TestFees = testFees;
        }

        public static BeltRank Create(string name, decimal testFees)
        {
            return new BeltRank(0, name, testFees);
        }

        public static BeltRank Load(int id, string name, decimal testFees)
        {
            return new BeltRank(id, name, testFees);
        }

        public void ChangeTestFees(decimal NewTestFees)
        {
            _ValidateFees(NewTestFees);
            TestFees = NewTestFees;
        }

        private void _ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Belt rank name cannot be null or empty");
        }

        private void _ValidateFees(decimal fees)
        {
            if (fees < 0) throw new DomainException("Belt test fees cannot be nigative");
        }
    }
}

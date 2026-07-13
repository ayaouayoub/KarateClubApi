using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using KarateClub.Domain.Exceptions;
using KarateClub.Domain.ValueObjs;

namespace KarateClub.Domain.Entities
{
    public class Person
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Address { get; private set; }
        public Email Email { get; private set; } = null!;

        private Person(int id, string name, string? address, Email email)
        {
            _ValidateName(name);
            _ValidateAddress(address);

            Id = id;
            Name = name;
            Address = address;
            Email = email;
        }

        public static Person Create(string name, string? address, Email email)
        {
            return new Person(-1, name, address, email);
        }

        public static Person Load(int id, string name, string? address, Email email)
        {
            return new Person(id, name, address, email);
        }

        public void ChangeName(string name)
        {
            _ValidateName(name);
            Name = name;
        }

        public void ChangeAddress(string address)
        {
            _ValidateAddress(address);
            Address = address;
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }

        private void _ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Person name cannot be null or empty");
        }

        private void _ValidateAddress(string? address)
        {
            if (address == "") throw new DomainException("Address cannot be empty");
        }
    }
}

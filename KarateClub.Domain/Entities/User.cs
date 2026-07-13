using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public sealed class User
    {
        private readonly HashSet<Permission> _permissions = [];

        private User(int id, string username, string passwordHash, bool isSuperAdmin, bool isActive, DateTime createdAt, int personId, Person? person)
        {
            _ValidateUsername(username);
            _ValidatePasswordHash(passwordHash);

            if (person is not null && person.Id != personId)
                throw new DomainException("Person id mismatch.");

            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            IsSuperAdmin = isSuperAdmin;
            IsActive = isActive;
            CreatedAt = createdAt;
            PersonId = personId;
            Person = person;
        }

        public int Id { get; }

        public string Username { get; private set; }

        public string PasswordHash { get; private set; }

        public bool IsSuperAdmin { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; }

        public int PersonId { get; }

        public Person? Person { get; }

        public IReadOnlyCollection<Permission> Permissions => _permissions;

        public static User Create(string username, string passwordHash, Person person)
        {
            ArgumentNullException.ThrowIfNull(person);

            return new User(
                id: 0,
                username: username,
                passwordHash: passwordHash,
                isSuperAdmin: false,
                isActive: true,
                createdAt: DateTime.UtcNow,
                personId: person.Id,
                person: person
            );
        }

        public static User Load(int id, string username, string passwordHash, bool isSuperAdmin, bool isActive, DateTime createdAt, int personId)
        {
            return new User(
                id,
                username,
                passwordHash,
                isSuperAdmin,
                isActive,
                createdAt,
                personId,
                null
            );
        }

        public static User LoadWithPerson(int id, string username, string passwordHash, bool isSuperAdmin, bool isActive, DateTime createdAt, Person person)
        {
            ArgumentNullException.ThrowIfNull(person);

            return new User(
                id,
                username,
                passwordHash,
                isSuperAdmin,
                isActive,
                createdAt,
                person.Id,
                person);
        }

        public void ChangeUsername(string username)
        {
            _ValidateUsername(username);

            if (Username == username)
                return;

            Username = username;
        }

        public void ChangePassword(string passwordHash)
        {
            _ValidatePasswordHash(passwordHash);

            if (PasswordHash == passwordHash)
                throw new DomainException("New password must be different.");

            PasswordHash = passwordHash;
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("User is already active.");

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("User is already inactive.");

            if (IsSuperAdmin)
                throw new DomainException("Super admin cannot be deactivated.");

            IsActive = false;
        }

        public void PromoteToSuperAdmin()
        {
            if (IsSuperAdmin)
                throw new DomainException("User is already super admin.");

            IsSuperAdmin = true;
        }
        public void SetPermissions(IEnumerable<Permission> permissions)
        {
            if (IsSuperAdmin)
            {
                throw new DomainException("Cannot add permissions to super admin");
            }

            ArgumentNullException.ThrowIfNull(permissions);

            _permissions.Clear();

            foreach (var permission in permissions)
                _permissions.Add(permission);
        }

        public void AddPermission(Permission permission)
        {
            if (IsSuperAdmin)
            {
                throw new DomainException("Cannot add permissions to super admin");
            }

            ArgumentNullException.ThrowIfNull(permission);
            _permissions.Add(permission);
        }

        private static void _ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("Username is required.");
        }

        private static void _ValidatePasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new DomainException("Password hash is required.");
        }
    }
}

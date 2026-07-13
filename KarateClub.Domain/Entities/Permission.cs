using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public class Permission
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = null!;

        private Permission(int id, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new DomainException("Permission code cannot be null");
            }

            Id = id;
            Code = code;
        }

        public static Permission Create(string code)
        {
            return new Permission(-1, code);
        }

        public static Permission Load(int id, string code)
        {
            return new Permission(id, code);
        }
    }
}

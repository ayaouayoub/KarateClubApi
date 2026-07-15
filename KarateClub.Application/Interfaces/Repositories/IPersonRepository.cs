using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetPersonByIdAsync(int id);
        Task<List<Person>> GetPeopleAsync();
        Task<bool> AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
    }
}

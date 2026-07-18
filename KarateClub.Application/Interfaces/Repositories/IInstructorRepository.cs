using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IInstructorRepository
    {
        Task<Instructor?> GetByIdAsync(int id);
        Task<Instructor?> GetByPersonIdAsync(int personId);
        Task<int> AddInstructorAsync(Instructor instructor);
        Task<List<Instructor>> GetInstructorsAsync();
        Task<bool> DeactivateInstructorAsync(int id);
        Task<bool> ActivateInstructorAsync(int id);
        Task UpdateCurrentBletRankAsync(int instructorId, int beltRankId);
    }
}

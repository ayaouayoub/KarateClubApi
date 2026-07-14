using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetUsersAsync();
        Task<bool> DeactivateUserAsync(int id);
        Task<int> AddUserAsync(User user);
        Task<User?> GetUserByPersonIdAsync(int personId);
        Task UpdateUserAsync(User user);
        Task<bool> ChangeMyUsernameAsync(int userId, string username);
        Task<bool> ChangeMyPasswordAsync(int userId, string password);
    }
}

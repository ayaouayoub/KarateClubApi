using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.User
{
    public class AddUserHandler
    {
        private readonly IUserRepository _repo;

        public AddUserHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        //public async Task<int> ExecuteAsync(CreateUserCommand createUserCommand)
        //{

        //    Domain.Entities.User.Create
        //    (
        //            createUserCommand.Username,
        //            createUserCommand.PasswordHash

        //     )
        //    _repo.AddUserAsync
        //    (
                
        //    );
        //}
    }
}

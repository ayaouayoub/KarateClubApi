using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Person
{
    public class GetPesronHandler
    {
        private readonly IPersonRepository _repo;

        public GetPesronHandler(IPersonRepository repo)
        {
            _repo = repo;
        }

        public async Task<PersonDto> ExecuteAsync(GetPesonbByIdQuery query)
        {
            var person = await _repo.GetPersonById(query.PersonId);

            if (person is null)
            {
                throw new NotFoundException("Person not found.");
            }

            return new PersonDto()
            {
                Id = person.Id,
                Address = person.Address,
                Name = person.Name,
                Email = person.Email.Value
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Exceptions;
using KarateClub.Application.Handlers.Person.Commnds;

namespace KarateClub.Application.Handlers.Person
{
    public class AddNewPersonHandler
    {
        private readonly IPersonRepository _personRepository;


        public AddNewPersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDto> ExecuteAsync(CreatePersonCommand command)
        {
            int newPersonId = await _personRepository.AddPersonAsync(_CreatePerson(command));
            return new PersonDto()
            {
                Id = newPersonId,
                Name = command.Name,
                Address = command.Address,
                Email = command.Email
            };
        }

        private Domain.Entities.Person _CreatePerson(CreatePersonCommand command)
        {
            return Domain.Entities.Person.Create(command.Name, command?.Address, new Domain.ValueObjs.Email(command?.Email ?? string.Empty));
        }
    }
}

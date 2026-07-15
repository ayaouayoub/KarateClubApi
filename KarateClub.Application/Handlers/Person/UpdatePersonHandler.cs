using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Person.Commnds;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Handlers.Person
{
    public class UpdatePersonHandler
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonDto> ExecuteAsync(UpdatePersonCommand command)
        {
            var person = await _personRepository.GetPersonByIdAsync(command.PersonId) ?? throw new NotFoundException("Person is not found");
            _ChangePersonObjectData(command, person);
            return await _SaveChanges(person);
        }

        private void _ChangePersonObjectData(UpdatePersonCommand command, Domain.Entities.Person person)
        {
            person.ChangeName(command.Name);
            person.ChangeAddress(command?.Address);
            person.ChangeEmail(new Domain.ValueObjs.Email(command?.Email));
        }

        private async Task<PersonDto> _SaveChanges(Domain.Entities.Person person)
        {
            await _personRepository.UpdatePersonAsync(person);
            return new PersonDto()
            {
                Id = person.Id,
                Name = person.Name,
                Address = person.Address,
                Email = person.Email?.Value
            };
        }
    }
}

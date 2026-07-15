using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Person
{
    public class GetPeopleHandler
    {
        private readonly IPersonRepository _personRepository;

        public GetPeopleHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<PersonDto>> ExecuteAsync(GetPeopleQuery query)
        {
            return _ConvertToPeopleDtos(await _personRepository.GetPeopleAsync());
        }

        private List<PersonDto> _ConvertToPeopleDtos(List<Domain.Entities.Person> people)
        {
            var peopleDtos = new List<PersonDto>();
            foreach (var person in people) peopleDtos.Add(new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Address = person.Address,
                Email = person.Email.Value
            });
            return peopleDtos;
        }
    }
}

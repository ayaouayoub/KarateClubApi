using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.Instructor
{
    public class CreateInstructorHandler
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IBeltRankRepository _beltRankRepository;

        public CreateInstructorHandler(IInstructorRepository instructorRepository, IPersonRepository personRepository, IBeltRankRepository beltRankRepository)
        {
            _instructorRepository = instructorRepository;
            _personRepository = personRepository;
            _beltRankRepository = beltRankRepository;
        }

        public async Task<InstructorDto> ExecuteAsync(CreateInstructorCommand command)
        {
            var person = await _PerparePerson(command);
            var belt = await _PerpareBeltRank(command);
            var instructor = Domain.Entities.Instructor.Create(person, command.Qualification, belt);
            int newInstructorId = await _instructorRepository.AddInstructorAsync(instructor);
            return new InstructorDto
            {
                Id = newInstructorId,
                CurrentBeltRankID = belt.Id,
                HireDate = instructor.HireDate,
                IsActive = instructor.IsActive,
                PersonID = person.Id,
                Qualification = instructor.Qualification
            };
        }

        private async Task<Domain.Entities.Person> _PerparePerson(CreateInstructorCommand command)
        {
            var person = await _personRepository.GetPersonByIdAsync(command.PersonId) ?? throw new NotFoundException("Person not found");
            if (await _instructorRepository.GetByPersonIdAsync(person.Id) is not null) throw new DomainException("this persone is associated with another instructor");
            return person;
        }

        private async Task<Domain.Entities.BeltRank> _PerpareBeltRank(CreateInstructorCommand command)
        {
            return await _beltRankRepository.GetByIdAsync(command.BeltRankID) ?? throw new NotFoundException("Belt rank not found");
        }
    }
}

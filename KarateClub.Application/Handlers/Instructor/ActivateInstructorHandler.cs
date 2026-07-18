using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class ActivateInstructorHandler
    {
        private readonly IInstructorRepository _instructorRepository;

        public ActivateInstructorHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task ExecuteAsync(ActivateInstructorCommand command)
        {
            var instructor = await _instructorRepository.GetByIdAsync(command.InstructorId) ?? throw new NotFoundException("Instructor not found.");

            instructor.Activate();

            if (!await _instructorRepository.ActivateInstructorAsync(command.InstructorId)) throw new Exception("Fialed to Activate instructor.");
        }
    }
}

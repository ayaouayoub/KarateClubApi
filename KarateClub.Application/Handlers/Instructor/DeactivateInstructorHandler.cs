using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class DeactivateInstructorHandler
    {
        private readonly IInstructorRepository _instructorRepository;

        public DeactivateInstructorHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task ExecuteAsync(DeactivateInstructorCommand command)
        {
            var instructor = await _instructorRepository.GetByIdAsync(command.InstructorId) ?? throw new NotFoundException("Instructor not found.");

            instructor.Deactivate();

            if (!await _instructorRepository.DeactivateInstructorAsync(command.InstructorId)) throw new Exception("Fialed to deactivate instructor.");
        }     
    }
}

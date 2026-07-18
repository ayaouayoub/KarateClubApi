using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class GetInstructorByPersonIdHandler
    {
        private readonly IInstructorRepository _instructorRepository;

        public GetInstructorByPersonIdHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorDto> ExecuteAsync(GetInstructorByPersonIdQuery query)
        {
            var instructor = await _instructorRepository.GetByPersonIdAsync(query.PersonId) ?? throw new NotFoundException("Instructor not found.");

            return new InstructorDto
            {
                Id = instructor.Id,
                Qualification = instructor.Qualification,
                HireDate = instructor.HireDate,
                IsActive = instructor.IsActive,
                CurrentBeltRankID = instructor.CurrentBeltRankID,
                PersonID = instructor.PersonId
            };
        }
    }
}

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
    public class GetInstructorHandler
    {
        private readonly IInstructorRepository _instructorRepository;

        public GetInstructorHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorDetailsDto> ExecuteAsync(GetInstructorQuery query)
        {
            var instructor = await _instructorRepository.GetByIdAsync(query.InstructorId) ?? throw new NotFoundException("Instructor not found.");

            return new InstructorDetailsDto
            {
                Id = instructor.Id,
                Qualification = instructor.Qualification,
                HireDate = instructor.HireDate,
                IsActive = instructor.IsActive,
                BeltRankDto = new BeltRankDto
                {
                    Id = instructor.CurrentBeltRankID,
                    Name = instructor.BeltRank.Name,
                    TestFees = instructor.BeltRank.TestFees
                },
                PersonDto = new PersonDto
                {
                    Id = instructor.PersonId,
                    Name = instructor.Person.Name,
                    Address = instructor.Person.Address,
                    Email = instructor.Person.Email.Value
                }
            };
        }
    }
}

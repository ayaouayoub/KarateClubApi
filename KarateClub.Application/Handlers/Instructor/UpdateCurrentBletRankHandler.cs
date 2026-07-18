using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class UpdateCurrentBletRankHandler
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IBeltRankRepository _beltRankRepository;

        public UpdateCurrentBletRankHandler(IInstructorRepository instructorRepository, IBeltRankRepository beltRankRepository)
        {
            _instructorRepository = instructorRepository;
            _beltRankRepository = beltRankRepository;
        }

        public async Task ExecuteAsync(UpdateCurrentBletRankCommand command)
        {
            var instructor = await _instructorRepository.GetByIdAsync(command.InstructorId) ?? throw new NotFoundException("Instructor not found.");
            var beltRank = await _beltRankRepository.GetByIdAsync(command.BeltRankId) ?? throw new NotFoundException("Belt rank not found.");

            instructor.ChangeBeltRank(beltRank);

            if (!await _instructorRepository.UpdateCurrentBletRankAsync(command.InstructorId, command.BeltRankId)) throw new Exception("Fialed to update instructor's blet rank.");
        }
    }
}

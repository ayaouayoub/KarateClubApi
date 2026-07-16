using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.BeltRank.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.BeltRank
{
    public class ChangeBeltTestFeesHandler
    {
        private readonly IBeltRankRepository _beltRankRepository;

        public ChangeBeltTestFeesHandler(IBeltRankRepository beltRankRepository)
        {
            _beltRankRepository = beltRankRepository;
        }

        public async Task ExecuteAsync(ChangeBeltTestFeesCommand command)
        {
            var blet = await _beltRankRepository.GetByIdAsync(command.BeltId) ?? throw new NotFoundException("Belt not found.");

            blet.ChangeTestFees(command.TestFees);

            if (!await _beltRankRepository.ChangeBeltTestFeesAsync(command.BeltId, command.TestFees))
            {
                throw new Exception("Failed to update test fees");
            }
        }
    }
}

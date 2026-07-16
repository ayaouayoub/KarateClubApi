using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Handlers.BeltRank
{
    public class GetBeltsHandler
    {
        private readonly IBeltRankRepository _beltRankRepository;

        public GetBeltsHandler(IBeltRankRepository beltRankRepository)
        {
            _beltRankRepository = beltRankRepository;
        }

        public async Task<IEnumerable<BeltRankDto>> ExecuteAsync(GetBeltsQuery query)
        {
            return _ConvertToDto(await _beltRankRepository.GetBeltsAsync());
        }

        private IEnumerable<BeltRankDto> _ConvertToDto(List<Domain.Entities.BeltRank> belts)
        {
            return belts.Select(b => new BeltRankDto()
            {
                Id = b.Id,
                Name = b.Name,
                TestFees = b.TestFees
            });
        }
    }
}

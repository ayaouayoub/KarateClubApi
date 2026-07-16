using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.BeltRank
{
    public class GetBeltRankHandler
    {
        private readonly IBeltRankRepository _beltRankRepository;

        public GetBeltRankHandler(IBeltRankRepository beltRankRepository)
        {
            _beltRankRepository = beltRankRepository;
        }

        public async Task<BeltRankDto> ExecuteAsync(GetBeltRankQuery query)
        {
            var beltRank = await _beltRankRepository.GetByIdAsync(query.BeltRankId) ?? throw new NotFoundException("Belt rank not found");
            return new BeltRankDto()
            {
                Id = beltRank.Id,
                Name = beltRank.Name,
                TestFees = beltRank.TestFees
            };
        }
    }
}

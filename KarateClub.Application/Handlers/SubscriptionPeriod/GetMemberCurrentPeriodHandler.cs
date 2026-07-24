using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.SubscriptionPeriod.Queries;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.SubscriptionPeriod
{
    public class GetMemberCurrentPeriodHandler
    {
        private readonly ISubscriptionPeriodRepository _subscriptionPeriodRepository;
        private readonly IMemberRepository _memberRepository;

        public GetMemberCurrentPeriodHandler(ISubscriptionPeriodRepository subscriptionPeriodRepository, IMemberRepository memberRepository)
        {
            _subscriptionPeriodRepository = subscriptionPeriodRepository;
            _memberRepository = memberRepository;
        }

        public async Task<SubscriptionPeriodDto> ExecuteAsync(GetMemberCurrentPeriodQuery query)
        {
            var member = await _memberRepository.GetByIdAsync(query.MemberId) ?? throw new NotFoundException("Member not found.");
            var subscriptionPeriod = await _subscriptionPeriodRepository.GetCurrentPeriodAsync(member.Id) ?? throw new NotFoundException("Member has no active subscription.");
            return new SubscriptionPeriodDto
            {
                Id = subscriptionPeriod.Id,
                EndDate = subscriptionPeriod.Period.EndDate,
                StartDate = subscriptionPeriod.Period.StartDate,
                MemberId = member.Id,
                TotalFees = subscriptionPeriod.TotalFees,
                SubscriptionPlanId = subscriptionPeriod.SubscriptionPlanId
            };
        }
    }
}

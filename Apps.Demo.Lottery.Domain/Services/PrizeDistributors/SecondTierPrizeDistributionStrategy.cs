using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Enums;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDistributors;

public class SecondTierPrizeDistributionStrategy(ISecondTierPrizeDecisionService secondTierPrizeDecisionService) : IPrizeDistributionStrategy
{
    decimal IPrizeDistributionStrategy.Distribute(List<Ticket> eligibleTickets, int totalTickets, decimal prizePool,
        PrizeDistributionResultDto resultDto)
    {
        var winners = secondTierPrizeDecisionService.DecideWinners(eligibleTickets, totalTickets).ToList(); //Converted to list to avoid the multiple enumeration

        var share = Math.Floor((prizePool / winners.Count) * 100) / 100m;
        var distributed = share * winners.Count;
        foreach (var ticket in winners)
        {
            ticket.SetPrize(PrizeTier.Second, share);
            resultDto.SecondTierWinners.Add((ticket, share));
        }
        eligibleTickets.RemoveAll(t => t.WinningTier == PrizeTier.Second);
        return distributed;
    }
}
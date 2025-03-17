using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDistributors;

public class ThirdTierPrizeDistributionStrategy(IThirdTierPrizeDecisionService thirdTierPrizeDecisionService) : IPrizeDistributionStrategy
{
    decimal IPrizeDistributionStrategy.Distribute(List<Ticket> eligibleTickets, int totalTickets, decimal prizePool, PrizeDistributionResultDto resultDto)
    {
        var winners = thirdTierPrizeDecisionService.DecideWinners(eligibleTickets, totalTickets).ToList();
        
        // Calculate equal share rounded down to 2 decimal places.
        var share = Math.Floor((prizePool / winners.Count) * 100) / 100m;
        var distributed = share * winners.Count;

        foreach (var ticket in winners)
        {
            ticket.SetPrize(PrizeTier.Third, share);
            resultDto.ThirdTierWinners.Add((ticket, share));
        }

        eligibleTickets.RemoveAll(t => t.WinningTier == PrizeTier.Third);
        return distributed;
    }
}
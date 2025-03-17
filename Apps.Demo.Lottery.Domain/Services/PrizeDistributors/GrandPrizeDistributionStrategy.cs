using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Enums;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDistributors;

public class GrandPrizeDistributionStrategy(IGrandPriceDecisionService grandPriceDecisionService) : IPrizeDistributionStrategy
{
    decimal IPrizeDistributionStrategy.Distribute(List<Ticket> eligibleTickets, int totalTickets, decimal prizePool,
        PrizeDistributionResultDto resultDto)
    {
        if (eligibleTickets.Count == 0)
        {
            return 0m;
        }

        var grandWinner = grandPriceDecisionService.DecideWinner(eligibleTickets); 
        grandWinner.SetPrize(PrizeTier.Grand, prizePool);
        resultDto.GrandPrizeWinner = grandWinner;
        resultDto.GrandPrizeAmount = grandWinner.PrizeAmount;
        eligibleTickets.Remove(grandWinner);
        return prizePool;
    }
}
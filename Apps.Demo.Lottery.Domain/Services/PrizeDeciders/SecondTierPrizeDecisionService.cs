using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDeciders;

public class SecondTierPrizeDecisionService(GameSettings settings) : ISecondTierPrizeDecisionService
{
    IEnumerable<Ticket> ISecondTierPrizeDecisionService.DecideWinners(List<Ticket> eligibleTickets, int totalTickets)
    {
        var expectedWinnerCount = (int)Math.Round(totalTickets * settings.SecondTierTicketRatio, MidpointRounding.AwayFromZero);
        var winnerCount = Math.Min(expectedWinnerCount, eligibleTickets.Count);
        return winnerCount <= 0 ? [] : eligibleTickets.Take(winnerCount);
    }
}
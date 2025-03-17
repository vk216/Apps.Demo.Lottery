using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDeciders;

public class ThirdTierPrizeDecisionService(GameSettings settings) : IThirdTierPrizeDecisionService
{
    IEnumerable<Ticket> IThirdTierPrizeDecisionService.DecideWinners(List<Ticket> eligibleTickets, int totalTickets)
    {
        var expectedWinnerCount = (int)Math.Round(totalTickets * settings.ThirdTierTicketRatio, MidpointRounding.AwayFromZero);
        var winnerCount = Math.Min(expectedWinnerCount, eligibleTickets.Count);
        return winnerCount <= 0 ? [] : eligibleTickets.Take(winnerCount);
    }
}
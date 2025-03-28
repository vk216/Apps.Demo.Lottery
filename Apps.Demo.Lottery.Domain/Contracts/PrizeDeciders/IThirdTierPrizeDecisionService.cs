using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;

public interface IThirdTierPrizeDecisionService
{
    IEnumerable<Ticket> DecideWinners(List<Ticket> eligibleTickets, int totalTickets);
}
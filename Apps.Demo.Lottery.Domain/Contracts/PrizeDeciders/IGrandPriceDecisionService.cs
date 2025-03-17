using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;

public interface IGrandPriceDecisionService
{
    Ticket DecideWinner(List<Ticket> eligibleTickets);
}
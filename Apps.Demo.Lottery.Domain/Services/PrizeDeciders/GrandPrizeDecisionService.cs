using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Services.PrizeDeciders;

public class GrandPrizeDecisionService : IGrandPriceDecisionService
{
    Ticket IGrandPriceDecisionService.DecideWinner(List<Ticket> eligibleTickets)
    {
        if (eligibleTickets is null || (eligibleTickets.Count == 0))
        {
            throw new DomainException("No eligible tickets were found.");
        }

        return eligibleTickets.First();
    }
}
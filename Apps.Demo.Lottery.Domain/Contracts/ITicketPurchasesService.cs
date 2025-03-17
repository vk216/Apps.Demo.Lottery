using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Contracts;

public interface ITicketPurchasesService
{
    void PurchaseTickets(Player player, int requestedTickets);
}
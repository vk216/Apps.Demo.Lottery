using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Services;

public class TicketPurchasesService(TicketSettings settings) : ITicketPurchasesService
{
    void ITicketPurchasesService.PurchaseTickets(Player player, int requestedTickets)
    {
        if (requestedTickets < settings.MinTicketsAllowed || requestedTickets > settings.MaxTicketsAllowed)
        {
            throw new DomainException(
                $"Ticket purchase must be between {settings.MinTicketsAllowed} and {settings.MaxTicketsAllowed}.");
        }

        var affordableTickets = (int)(player.Balance / settings.Cost);
        var ticketsToPurchase = Math.Min(requestedTickets, affordableTickets);

        if (ticketsToPurchase <= 0)
        {
            throw new DomainException("Insufficient balance to purchase any tickets.");
        }

        for (var index = 0; index < ticketsToPurchase; index++)
        {
            var ticket = new Ticket(player);
            player.AddTicket(ticket);
            player.ReduceBalance(settings.Cost);
        }
    }
}
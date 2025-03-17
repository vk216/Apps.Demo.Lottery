using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Entities;

public class Ticket
{
    private static int _ticketCounter = 0;
    public int TicketNumber { get; }
    public Player Owner { get; }
    public PrizeTier WinningTier { get; private set; } = PrizeTier.None;
    public decimal PrizeAmount { get; private set; } = 0m;

    public Ticket(Player owner)
    {
        TicketNumber = ++_ticketCounter;
        Owner = owner ?? throw new DomainException("There should be an owner for a ticket.");
    }

    public void SetPrize(PrizeTier tier, decimal amount)
    {
        if (WinningTier != PrizeTier.None)
        {
            throw new DomainException("This ticket has already won a prize.");
        }
        WinningTier = tier;
        PrizeAmount = amount;
    }
}
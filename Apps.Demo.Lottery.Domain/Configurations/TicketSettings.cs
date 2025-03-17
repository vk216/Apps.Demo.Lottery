namespace Apps.Demo.Lottery.Domain.Configurations;

public record TicketSettings
{
    public int MinTicketsAllowed { get; init; }
    public int MaxTicketsAllowed { get; init; }
    public decimal Cost { get; init; }
}
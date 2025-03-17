namespace Apps.Demo.Lottery.Domain.Configurations;

public record GameSettings
{
    public int MinNumberOfPlayers { get; init; }
    public int MaxNumberOfPlayers { get; init; }
    public decimal GrandPrizePercentage { get; init; }
    public decimal SecondTierPercentage { get; init; }
    public decimal ThirdTierPercentage { get; init; } 

    public double SecondTierTicketRatio { get; init; }
    public double ThirdTierTicketRatio { get; init; }
}
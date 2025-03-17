using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.DTOs;

public class PrizeDistributionResultDto
{
    public Ticket GrandPrizeWinner { get; set; } = null!;
    public decimal GrandPrizeAmount { get; set; }
    public List<(Ticket Ticket, decimal PrizeAmount)> SecondTierWinners { get; set; } = [];
    public List<(Ticket Ticket, decimal PrizeAmount)> ThirdTierWinners { get; set; } = [];
    public decimal HouseProfit { get; set; }
}
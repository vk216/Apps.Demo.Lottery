using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;

public interface IPrizeDistributionStrategy
{
    decimal Distribute(List<Ticket> eligibleTickets, int totalTickets, decimal prizePool, PrizeDistributionResultDto resultDto);
}
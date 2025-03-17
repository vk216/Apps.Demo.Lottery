using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using Apps.Demo.Lottery.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Demo.Lottery.Application.Services;

public class PrizeDistributionOrchestrator(GameSettings gameSettings, TicketSettings ticketSettings,
    [FromKeyedServices("GrandTier")] IPrizeDistributionStrategy grandPrizeStrategy
    , [FromKeyedServices("SecondTier")] IPrizeDistributionStrategy secondTierStrategy
    , [FromKeyedServices("ThirdTier")] IPrizeDistributionStrategy thirdTierStrategy) : IPrizeDistributionOrchestrator
{
        public PrizeDistributionResultDto DistributePrizes(Game game)
        {
            if (game is null)
            {
                throw new DomainException("Game cannot be null.");
            }

            var result = new PrizeDistributionResultDto();
            var allTickets = game.GetAllTickets();
            var totalTickets = allTickets.Count;
            var totalRevenue = totalTickets * ticketSettings.Cost;

            var grandPrizePool = totalRevenue * gameSettings.GrandPrizePercentage;
            var secondTierPool = totalRevenue * gameSettings.SecondTierPercentage;
            var thirdTierPool = totalRevenue * gameSettings.ThirdTierPercentage;

            var totalDistributed = 0m;
            List<Ticket> eligibleTickets = [..allTickets];

            totalDistributed += grandPrizeStrategy.Distribute(eligibleTickets, totalTickets, grandPrizePool, result);
            totalDistributed += secondTierStrategy.Distribute(eligibleTickets, totalTickets, secondTierPool, result);
            totalDistributed += thirdTierStrategy.Distribute(eligibleTickets, totalTickets, thirdTierPool, result);

            result.HouseProfit = totalRevenue - totalDistributed;
            return result;
        }
    }
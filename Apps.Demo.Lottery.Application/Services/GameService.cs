using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using Apps.Demo.Lottery.Application.Contracts;

namespace Apps.Demo.Lottery.Application.Services;

public class GameService(
    GameSettings gameSettings,
    TicketSettings ticketSettings,
    PlayerSettings playerSettings,
    ITicketPurchasesService ticketPurchasesService,
    IPrizeDistributionOrchestrator prizeDistributionOrchestrator)
    : IGameService
{
    private readonly Random _random = new();

    Game IGameService.InitializeGame(int noOfTickets)
    {
        var game = new Game();
        var humanPlayer = new Player("Player 1", playerSettings.StartingBalance);
        ticketPurchasesService.PurchaseTickets(humanPlayer, noOfTickets);
        game.AddPlayer(humanPlayer);

        var totalPlayers = _random.Next(gameSettings.MinNumberOfPlayers, gameSettings.MaxNumberOfPlayers + 1);
        for (var index = 2; index <= totalPlayers; index++)
        {
            var cpuPlayer = new Player($"Player {index}", playerSettings.StartingBalance);
            var cpuTicketRequest = _random.Next(ticketSettings.MinTicketsAllowed,
                ticketSettings.MaxTicketsAllowed + 1);
            ticketPurchasesService.PurchaseTickets(cpuPlayer, cpuTicketRequest);
            game.AddPlayer(cpuPlayer);
        }

        return game;
    }

    PrizeDistributionResultDto IGameService.RunLottery(Game game)
    {
        if (game == null)
        {
            throw new DomainException("Game has not been initialized.");
        }

        return prizeDistributionOrchestrator.DistributePrizes(game);
    }
}
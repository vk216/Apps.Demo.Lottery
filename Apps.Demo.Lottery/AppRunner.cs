using Apps.Demo.Lottery.Application.Contracts;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery;

public class AppRunner(IGameService gameService)
{
    public void Run()
    {
        Console.Write("Enter the number of tickets to purchase (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out var ticketRequest))
        {
            Console.WriteLine("Invalid input. Exiting...");
            return;
        }

        try
        {
            var game = gameService.InitializeGame(ticketRequest);
            DisplayPlayers(game);
            var result = gameService.RunLottery(game);
            DisplayWinners(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void DisplayPlayers(Game game)
    {
        Console.WriteLine("Players and their purchased tickets:");
        foreach (var player in game.Players)
        {
            Console.WriteLine($"{player.Name} - Tickets: {player.Tickets.Count}");
        }
    }

    private void DisplayWinners(PrizeDistributionResultDto result)
    {
        Console.WriteLine("--- Ticket Draw Results ---");

        if (result.GrandPrizeWinner != null)
        {
            Console.WriteLine($"Grand Prize Winner: {result.GrandPrizeWinner.Owner.Name} " +
                              $"(Ticket #{result.GrandPrizeWinner.TicketNumber}) won ${result.GrandPrizeAmount}");
        }
        else
        {
            Console.WriteLine("No Grand Prize Winner.");
        }

        Console.WriteLine("Second Tier Winners:");
        if (result.SecondTierWinners.Count != 0)
        {
            foreach (var (ticket, prize) in result.SecondTierWinners)
            {
                Console.WriteLine($"{ticket.Owner.Name} (Ticket #{ticket.TicketNumber}) won ${prize}");
            }
        }
        else
        {
            Console.WriteLine("No Second Tier Winners.");
        }

        Console.WriteLine("Third Tier Winners:");
        if (result.ThirdTierWinners.Count != 0)
        {
            foreach (var (ticket, prize) in result.ThirdTierWinners)
            {
                Console.WriteLine($"{ticket.Owner.Name} (Ticket #{ticket.TicketNumber}) won ${prize}");
            }
        }
        else
        {
            Console.WriteLine("No Third Tier Winners.");
        }

        Console.WriteLine($"House Profit: ${result.HouseProfit}");
    }
}
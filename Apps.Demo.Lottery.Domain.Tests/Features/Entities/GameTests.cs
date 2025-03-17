using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Tests.Features.Entities;

[TestFixture]
public class GameTests
{
    [Test]
    public void AddPlayer_WithValidPlayer_ShouldAddPlayerToGame()
    {
        // Arrange
        var game = new Game();
        var player = new Player("1", 10m);

        // Act
        game.AddPlayer(player);

        // Assert
        Assert.That(game.Players, Has.Count.EqualTo(1), "Game should contain one player after adding one player.");
        Assert.That(game.Players, Has.Member(player));
    }

    [Test]
    public void AddPlayer_WithNullPlayer_ShouldThrowDomainException()
    {
        // Arrange
        var game = new Game();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => game.AddPlayer(null!));
        Assert.That(ex.Message, Is.EqualTo("Player cannot be null."));
    }

    [Test]
    public void GetAllTickets_ShouldReturnAllTicketsFromAllPlayers()
    {
        // Arrange
        var game = new Game();
        var player1 = new Player("Player 1", 10m);
        var player2 = new Player("Player 2", 10m);

        // Create some tickets for player1
        var ticket1 = new Ticket(player1);
        var ticket2 = new Ticket(player1);
        player1.AddTicket(ticket1);
        player1.AddTicket(ticket2);

        // Create one ticket for player2
        var ticket3 = new Ticket(player2);
        player2.AddTicket(ticket3);

        // Add players to game
        game.AddPlayer(player1);
        game.AddPlayer(player2);

        // Act
        var allTickets = game.GetAllTickets();

        // Assert
        Assert.That(allTickets, Has.Count.EqualTo(3), "Total number of tickets should be 3.");
        Assert.That(allTickets, Contains.Item(ticket1));
        Assert.That(allTickets, Contains.Item(ticket2));
        Assert.That(allTickets, Contains.Item(ticket3));
    }
}
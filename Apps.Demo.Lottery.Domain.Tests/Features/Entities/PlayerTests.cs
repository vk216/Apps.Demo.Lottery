using System.Collections;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using static NUnit.Framework.Assert;

namespace Apps.Demo.Lottery.Domain.Tests.Features.Entities;

[TestFixture]
public class PlayerTests
{
    [Test]
    public void Constructor_WithValidName_ShouldSetNameAndBalance()
    {
        // Arrange
        var playerName = "1";
        var startingBalance = 10m;
            
        // Act
        var player = new Player(playerName, startingBalance);
            
        // Assert
        That(player.Name, Is.EqualTo(playerName));
        That(player.Balance, Is.EqualTo(startingBalance));
        IsNotNull(player.Tickets);
        That(player.Tickets.Count, Is.EqualTo(0));
    }
        
    [Test]
    public void Constructor_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var emptyName = string.Empty;
        var startingBalance = 10m;
            
        // Act & Assert
        var ex = Throws<DomainException>(() => new Player(emptyName, startingBalance));
        That(ex?.Message, Is.EqualTo("Player name cannot be empty."));
    }
        
    [Test]
    public void AddTicket_ShouldIncreaseTicketCount()
    {
        // Arrange
        var player = new Player("1", 10m);
        var ticket = new Ticket(player);
            
        // Act
        player.AddTicket(ticket);
            
        // Assert
        That(player.Tickets, Has.Count.EqualTo(1));
        Contains(ticket, (ICollection)player.Tickets);
    }
        
    [Test]
    public void ReduceBalance_ShouldSubtractAmountFromBalance()
    {
        // Arrange
        var player = new Player("1", 10m);
        var amountToReduce = 3m;
            
        // Act
        player.ReduceBalance(amountToReduce);
            
        // Assert
        That(player.Balance, Is.EqualTo(7m));
    }
}
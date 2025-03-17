using System.Reflection;
using Apps.Demo.Lottery.Domain;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using static NUnit.Framework.Assert;
using Contains = NUnit.Framework.Contains;

namespace Apps.Demo.Lottery.Domain.Tests.Features.Entities;

[TestFixture]
public class TicketTests
{
    [SetUp]
    public void SetUp()
    {
        var field = typeof(Ticket).GetField("_ticketCounter", BindingFlags.Static | BindingFlags.NonPublic);
        field!.SetValue(null, 0); //Resetting in each run as we are maintaining a static counter.
    }

    [Test]
    public void Constructor_WithValidOwner_ShouldSetOwnerAndTicketNumber()
    {
        // Arrange
        var player = new Player("1", 10m);

        // Act
        var ticket = new Ticket(player);
        Multiple(() =>
        {
            That(ticket.Owner, Is.EqualTo(player), "Ticket owner should be set correctly.");
            That(ticket.TicketNumber, Is.EqualTo(1), "The first ticket should have TicketNumber 1.");
        });
    }

    [Test]
    public void Constructor_WithNullOwner_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var ex = Throws<DomainException>(() => new Ticket(null!));
        Assert.That(ex?.Message, Contains.Substring("There should be an owner for a ticket."));
    }

    [Test]
    public void SetPrize_WhenNotAlreadySet_ShouldAssignTierAndPrizeAmount()
    {
        // Arrange
        var player = new Player("2", 10m);
        var ticket = new Ticket(player);
        var prizeAmount = 100m;

        // Act
        ticket.SetPrize(PrizeTier.Grand, prizeAmount);
        Multiple(() =>
        {
            // Assert
            That(ticket.WinningTier, Is.EqualTo(PrizeTier.Grand), "Winning tier should be set to Grand.");
            That(ticket.PrizeAmount, Is.EqualTo(prizeAmount), "Prize amount should be set correctly.");
        });
    }

    [Test]
    public void SetPrize_WhenAlreadySet_ShouldThrowDomainException()
    {
        // Arrange
        var player = new Player("Charlie", 10m);
        var ticket = new Ticket(player);
        ticket.SetPrize(PrizeTier.Second, 50m);

        // Act & Assert
        var ex = Throws<DomainException>(() => ticket.SetPrize(PrizeTier.Third, 30m));
        That(ex?.Message, Is.EqualTo("This ticket has already won a prize."));
    }
}
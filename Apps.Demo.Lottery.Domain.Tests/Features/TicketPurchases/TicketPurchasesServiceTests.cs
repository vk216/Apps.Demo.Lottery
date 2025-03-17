using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using Apps.Demo.Lottery.Domain.Services;
using static NUnit.Framework.Assert;

namespace Apps.Demo.Lottery.Domain.Tests.Features.TicketPurchases;

[TestFixture]
public class TicketPurchasesServiceTests
{
    private TicketSettings _settings;
    private ITicketPurchasesService _ticketPurchasesService;

    [SetUp]
    public void Setup()
    {
        _settings = new TicketSettings
        {
            MinTicketsAllowed = 1,
            MaxTicketsAllowed = 10,
            Cost = 1.0m
        };

        _ticketPurchasesService = new TicketPurchasesService(_settings);
    }

    [Test]
    public void PurchaseTickets_WithValidRequest_Should_AddTicketsAndReduceBalance()
    {
        // Arrange
        var player = new Player("1", 10m);
        var requestedTickets = 5;

        // Act
        _ticketPurchasesService.PurchaseTickets(player, requestedTickets);

        // Assert
        Multiple(() =>
        {
            That(player.Tickets.Count, Is.EqualTo(5), "Player should have 5 tickets.");
            That(player.Balance, Is.EqualTo(5m), "Player balance should be reduced by 5 (5 tickets at $1 each).");
        });
    }

    [Test]
    public void PurchaseTickets_WithRequestedBelowMinimum_ShouldThrowDomainException()
    {
        // Arrange
        var player = new Player("1", 10m);
        int requestedTickets = 0; // Below the minimum allowed of 1

        // Act & Assert
        var ex = Throws<DomainException>(() => _ticketPurchasesService.PurchaseTickets(player, requestedTickets));
        That(ex?.Message,
            Is.EqualTo(
                $"Ticket purchase must be between {_settings.MinTicketsAllowed} and {_settings.MaxTicketsAllowed}."));
    }

    [Test]
    public void PurchaseTickets_WithRequestedAboveMaximum_ShouldThrowDomainException()
    {
        // Arrange
        var player = new Player("1", 10m);
        var requestedTickets = 15; // Above the maximum allowed of 10

        // Act & Assert
        var ex = Throws<DomainException>(() => _ticketPurchasesService.PurchaseTickets(player, requestedTickets));
        That(ex?.Message,
            Is.EqualTo(
                $"Ticket purchase must be between {_settings.MinTicketsAllowed} and {_settings.MaxTicketsAllowed}."));
    }

    [Test]
    public void PurchaseTickets_WithInsufficientBalance_ShouldThrowDomainException()
    {
        // Arrange
        var player = new Player("1", 0.5m); // Not enough balance to buy even one ticket
        var requestedTickets = 1;

        // Act & Assert
        var ex = Throws<DomainException>(() => _ticketPurchasesService.PurchaseTickets(player, requestedTickets));
        That(ex?.Message, Is.EqualTo("Insufficient balance to purchase any tickets."));
    }

    [Test]
    public void PurchaseTickets_WhenRequestedExceedsAffordable_ShouldPurchaseOnlyAffordableTickets()
    {
        // Arrange
        var player = new Player("1", 3m);
        var requestedTickets = 5;

        // Act
        _ticketPurchasesService.PurchaseTickets(player, requestedTickets);

        // Assert
        // Expect only 3 tickets to be purchased.
        Multiple(() =>
        {
            That(player.Tickets.Count, Is.EqualTo(3),
                "Player should have only 3 tickets because balance allows only 3.");
            That(player.Balance, Is.EqualTo(0m), "Player's balance should be fully reduced to 0.");
        });
    }
}
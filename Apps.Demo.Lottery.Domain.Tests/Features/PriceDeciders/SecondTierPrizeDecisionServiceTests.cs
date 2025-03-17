using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Services.PrizeDeciders;
using static NUnit.Framework.Assert;

namespace Apps.Demo.Lottery.Domain.Tests.Features.PriceDeciders;

[TestFixture]
public class SecondTierPrizeDecisionServiceTests
{
    private ISecondTierPrizeDecisionService _service;
    private GameSettings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new GameSettings { SecondTierTicketRatio = 0.1 }; //Adding only the necessary value
        _service = new SecondTierPrizeDecisionService(_settings);
    }

    [Test]
    public void DecideWinners_WithEmptyEligibleTickets_ReturnsEmpty()
    {
        // Arrange
        var eligibleTickets = new List<Ticket>();
        var totalTickets = 100;

        // Act
        var winners = _service.DecideWinners(eligibleTickets, totalTickets);
        That(winners, Is.Empty, "When no eligible tickets exist, the result should be empty.");
    }

    [Test]
    public void DecideWinners_WhenTotalTicketsProductRoundsToZero_ReturnsEmpty()
    {
        // Arrange
        var totalTickets = 4;
        var player = new Player("1", 10m);
        var eligibleTickets = new List<Ticket>
        {
            new(player),
            new(player)
        };

        // Act
        var winners = _service.DecideWinners(eligibleTickets, totalTickets);
        That(winners, Is.Empty, "Expected no winners when totalTickets * ratio rounds to 0.");
    }

    [Test]
    public void DecideWinners_WithMoreEligibleThanExpected_ReturnsExpectedCount()
    {
        // Arrange
        var totalTickets = 100;
        var player = new Player("2", 10m);
        // Create 15 tickets.
        var eligibleTickets = Enumerable.Range(1, 15).Select(_ => new Ticket(player)).ToList();

        // Act
        var winners = _service.DecideWinners(eligibleTickets, totalTickets).ToList();
        That(winners, Has.Count.EqualTo(10), "Expected exactly 10 winners when there are more eligible tickets.");
        CollectionAssert.AreEqual(eligibleTickets.Take(10), winners,
            "Winners should be the first 10 tickets from the eligible list.");
    }

    [Test]
    public void DecideWinners_WithFewerEligibleThanExpected_ReturnsAllEligible()
    {
        // Arrange
        var totalTickets = 100; // expectedWinnerCount would be 10.
        var player = new Player("Test Player", 10m);
        var eligibleTickets = Enumerable.Range(1, 8).Select(_ => new Ticket(player)).ToList();

        // Act
        var winners = _service.DecideWinners(eligibleTickets, totalTickets).ToList();
        That(winners, Has.Count.EqualTo(8), "When fewer eligible tickets exist than expected, all should be returned.");
        CollectionAssert.AreEqual(eligibleTickets, winners, "All eligible tickets should be returned as winners.");
    }
}
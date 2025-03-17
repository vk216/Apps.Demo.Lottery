using System.Reflection;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using Apps.Demo.Lottery.Domain.Services.PrizeDeciders;

namespace Apps.Demo.Lottery.Domain.Tests.Features.PriceDeciders;

[TestFixture]
public class GrandPrizeDecisionServiceTests
{
    private IGrandPriceDecisionService _grandPrizeDecisionService;
    private const string ExceptionMessage = "No eligible tickets were found.";

    [SetUp]
    public void Setup()
    {
        _grandPrizeDecisionService = new GrandPrizeDecisionService();
        var field = typeof(Ticket).GetField("_ticketCounter", BindingFlags.Static | BindingFlags.NonPublic);
        field!.SetValue(null, 0);
    }

    [Test]
    public void EligibleTickets_GrandPrizeDecision_Should_Return_FirstTicket()
    {
        //Arrange
        var player = new Player("1", 10m);
        var tickets = Enumerable.Range(1, 10).Select(_ => new Ticket(player)).ToList();
        var firstTicket = tickets.Single(t => t.TicketNumber == 1);

        //Act
        var winner = _grandPrizeDecisionService.DecideWinner(tickets);

        //Assert
        Assert.That(firstTicket, Is.EqualTo(winner));
    }

    [Test]
    public void EmptyTickets_GrandPrizeDecision_Should_Throw_DomainException()
    {
        //Arrange 
        var tickets = new List<Ticket>();

        //Act & Assert
        var ex = Assert.Throws<DomainException>(() => _grandPrizeDecisionService.DecideWinner(tickets));
        Assert.That(ex?.Message, Is.EqualTo(ExceptionMessage));
    }

    [Test]
    public void NullTickets_GrandPrizeDecision_Should_Throw_DomainException()
    {
        List<Ticket>? tickets = null;

        //Act & Assert
        var ex = Assert.Throws<DomainException>(() => _grandPrizeDecisionService.DecideWinner(tickets!));
        Assert.That(ex?.Message, Is.EqualTo(ExceptionMessage));
    }
}
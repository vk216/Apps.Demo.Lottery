using Apps.Demo.Lottery.Application.Contracts;
using Apps.Demo.Lottery.Application.Services;
using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;
using Apps.Demo.Lottery.Domain.Exceptions;
using Moq;
using static NUnit.Framework.Assert;

namespace Apps.Demo.Lottery.Application.Tests.Features.Game;

[TestFixture]
public class GameServiceTests
{
    private GameSettings _gameSettings;
    private TicketSettings _ticketSettings;
    private PlayerSettings _playerSettings;
    private Mock<ITicketPurchasesService> _ticketPurchasesServiceMock;
    private Mock<IPrizeDistributionOrchestrator> _prizeDistributionOrchestratorMock;
    private IGameService _gameService;

    [SetUp]
    public void Setup()
    {
        _gameSettings = new GameSettings { MinNumberOfPlayers = 10, MaxNumberOfPlayers = 15 };
        _ticketSettings = new TicketSettings { MinTicketsAllowed = 1, MaxTicketsAllowed = 10, Cost = 1.0m };
        _playerSettings = new PlayerSettings { StartingBalance = 10m };

        _ticketPurchasesServiceMock = new Mock<ITicketPurchasesService>();
        _ticketPurchasesServiceMock.Setup(x => x.PurchaseTickets(It.IsAny<Player>(), It.IsAny<int>()))
            .Callback<Player, int>((player, requestedTickets) =>
            {
                for (int i = 0; i < requestedTickets; i++)
                {
                    var ticket = new Ticket(player);
                    player.AddTicket(ticket);
                    player.ReduceBalance(_ticketSettings.Cost);
                }
            });

        _prizeDistributionOrchestratorMock = new Mock<IPrizeDistributionOrchestrator>();
        _prizeDistributionOrchestratorMock.Setup(x => x.DistributePrizes(It.IsAny<Domain.Entities.Game>()))
            .Returns(new PrizeDistributionResultDto());

        _gameService = new GameService(_gameSettings, _ticketSettings, _playerSettings,
            _ticketPurchasesServiceMock.Object, _prizeDistributionOrchestratorMock.Object);
    }

    [Test]
    public void InitializeGame_Should_CreateGameWithHumanAndCpuPlayers()
    {
        // Arrange
        var noOfTickets = 5;

        // Act
        var game = _gameService.InitializeGame(noOfTickets);

        //Assert
        Multiple(() =>
        {
            That(game.Players, Is.Not.Empty, "Game should have players.");

            var humanPlayer = game.Players.FirstOrDefault(p => p.Name == "Player 1");
            That(humanPlayer, Is.Not.Null, "Human player should be present.");
            Multiple(() =>
            {
                That(humanPlayer?.Tickets.Count, Is.EqualTo(noOfTickets),
                            "Human player should have the requested number of tickets.");
            });

            Multiple(() =>
            {
                That(game.Players, Has.Count.GreaterThanOrEqualTo(_gameSettings.MinNumberOfPlayers),
                    $"Game should have at least {_gameSettings.MinNumberOfPlayers} players.");
                That(game.Players, Has.Count.LessThanOrEqualTo(_gameSettings.MaxNumberOfPlayers),
                    $"Game should have no more than {_gameSettings.MaxNumberOfPlayers} players.");
            });
        });

        _ticketPurchasesServiceMock.Verify(x => x.PurchaseTickets(It.IsAny<Player>(), It.IsAny<int>()),
            Times.Exactly(game.Players.Count));
    }

    [Test]
    public void RunLottery_WithNullGame_ShouldThrowDomainException()
    {
        // Act & Assert
        var ex = Throws<DomainException>(() => _gameService.RunLottery(null!));
        That(ex?.Message, Is.EqualTo("Game has not been initialized."));
    }

    [Test]
    public void RunLottery_WithValidGame_Should_CallPrizeDistributionOrchestrator_AndReturnResult()
    {
        // Arrange
        var noOfTickets = 5;
        var game = _gameService.InitializeGame(noOfTickets);

        // Act
        var result = _gameService.RunLottery(game);

        // Assert
        _prizeDistributionOrchestratorMock.Verify(x => x.DistributePrizes(game), Times.Once,
            "The orchestrator should be called exactly once.");
        That(result, Is.TypeOf<PrizeDistributionResultDto>(),
            "The result should be of type PrizeDistributionResultDto.");
    }
}
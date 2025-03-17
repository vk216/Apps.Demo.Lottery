using Apps.Demo.Lottery.Domain.DTOs;
using Apps.Demo.Lottery.Domain.Entities;

namespace Apps.Demo.Lottery.Application.Contracts;

public interface IGameService
{
    Game InitializeGame(int noOfTickets);
    PrizeDistributionResultDto RunLottery(Game game);
}
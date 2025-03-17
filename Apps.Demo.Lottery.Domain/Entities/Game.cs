using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Entities;

public class Game
{
    private readonly List<Player> _players = [];
    public IReadOnlyCollection<Player> Players => _players.AsReadOnly();

    public void AddPlayer(Player player)
    {
        if (player is null)
        {
            throw new DomainException("Player cannot be null.");
        }

        _players.Add(player);
    }

    public List<Ticket> GetAllTickets()
    {
        return _players.SelectMany(p => p.Tickets).ToList();
    }
}
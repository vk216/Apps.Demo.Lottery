using Apps.Demo.Lottery.Domain.Exceptions;

namespace Apps.Demo.Lottery.Domain.Entities;

    public class Player
    {
        public string Name { get; }
        public decimal Balance { get; private set; }

        private readonly List<Ticket> _tickets = [];
        public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

        public Player(string name, decimal startingBalance)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Player name cannot be empty.");
            }

            Name = name;
            Balance = startingBalance;
        }

        public void AddTicket(Ticket ticket)
        {
            _tickets.Add(ticket);
        }
        public void ReduceBalance(decimal amount)
        {
            Balance -= amount;
        }
    }
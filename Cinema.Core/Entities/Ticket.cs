using Cinema.Core.Enums;

namespace Cinema.Core.Entities
{
    public class Ticket
    {
        public long Id { get; set; }

        public int Row { get; set; }
        public int SeatNumber { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public TicketStatus Status { get; set; } = TicketStatus.Booked;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
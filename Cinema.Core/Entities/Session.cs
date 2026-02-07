using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Cinema.Core.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        public int HallId { get; set; }
        public Hall Hall { get; set; } = null!;

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
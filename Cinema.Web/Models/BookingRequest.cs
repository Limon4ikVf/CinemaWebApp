namespace Cinema.Web.Models
{
    public class BookingRequest
    {
        public int SessionId { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class SeatDto
    {
        public int Row { get; set; }
        public int Place { get; set; }
    }
}
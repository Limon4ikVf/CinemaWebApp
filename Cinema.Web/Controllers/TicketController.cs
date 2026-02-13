using Cinema.Core.Entities;
using Cinema.Core.Enums;
using Cinema.Infrastructure.Data;
using Cinema.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly CinemaDbContext _context;

        public TicketController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int sessionId)
        {
            var session = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                return NotFound();
            }

            var soldTickets = await _context.Tickets
                .Where(t => t.SessionId == sessionId)
                .ToListAsync();

            ViewBag.SoldTickets = soldTickets;

            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> BookTickets([FromBody] BookingRequest request)
        {
            if (request == null || request.Seats == null || !request.Seats.Any())
            {
                return BadRequest("Не вибрано жодного місця.");
            }

            var session = await _context.Sessions.FindAsync(request.SessionId);
            if (session == null) return NotFound("Сеанс не знайдено.");

            var occupiedSeats = await _context.Tickets
                .Where(t => t.SessionId == request.SessionId)
                .ToListAsync();

            foreach (var seat in request.Seats)
            {
                if (occupiedSeats.Any(t => t.Row == seat.Row && t.SeatNumber == seat.Place))
                {
                    return Conflict($"Місце (Ряд {seat.Row}, Місце {seat.Place}) вже зайняте.");
                }
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Будь ласка, увійдіть у систему, щоб купити квиток.");
            }
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdString);

            var newTickets = new List<Ticket>();
            foreach (var seat in request.Seats)
            {
                newTickets.Add(new Ticket
                {
                    SessionId = request.SessionId,
                    Row = seat.Row,
                    SeatNumber = seat.Place,
                    Price = session.Price,
                    PurchaseDate = DateTime.UtcNow,
                    Status = TicketStatus.Booked,
                    UserId = userId
                });
            }

            await _context.Tickets.AddRangeAsync(newTickets);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Квитки успішно заброньовано!" });
        }
    }
}
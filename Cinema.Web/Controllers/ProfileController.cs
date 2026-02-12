using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly CinemaDbContext _context;

        public ProfileController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // тимчасово
            int currentUserId = 1;

            var tickets = await _context.Tickets
                .Include(t => t.Session)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s.Hall)
                .Where(t => t.UserId == currentUserId)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();

            return View(tickets);
        }
    }
}
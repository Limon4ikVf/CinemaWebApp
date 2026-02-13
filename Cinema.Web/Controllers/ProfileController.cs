using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CinemaDbContext _context;

        public ProfileController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = int.Parse(userIdString);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            if (user != null)
            {
                ViewBag.FullName = $"{user.FirstName} {user.LastName}";
            }

            var tickets = await _context.Tickets
                .Include(t => t.Session)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.Session)
                    .ThenInclude(s => s.Hall)
                .Where(t => t.UserId == currentUserId)
                .OrderByDescending(t => t.Session.StartTime)
                .ToListAsync();

            return View(tickets);
        }
    }
}
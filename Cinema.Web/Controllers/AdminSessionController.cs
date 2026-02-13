using Cinema.Core.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminSessionController : Controller
    {
        private readonly CinemaDbContext _context;

        public AdminSessionController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            return View(sessions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title");
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name");

            var defaultSession = new Session { StartTime = DateTime.Today.AddDays(1).AddHours(18) };

            return View(defaultSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Session session)
        {
            ModelState.Remove("Movie");
            ModelState.Remove("Hall");
            ModelState.Remove("Tickets");

            if (ModelState.IsValid)
            {
                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title", session.MovieId);
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name", session.HallId);
            return View(session);
        }
    }
}
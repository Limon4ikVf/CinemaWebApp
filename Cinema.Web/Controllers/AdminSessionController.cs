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

        [HttpGet]
        public async Task<IActionResult> Index(int? movieId, int? hallId)
        {
            var query = _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .AsQueryable();


            if (movieId.HasValue)
            {
                query = query.Where(s => s.MovieId == movieId.Value);
            }

            if (hallId.HasValue)
            {
                query = query.Where(s => s.HallId == hallId.Value);
            }

            var sessions = await query.OrderByDescending(s => s.StartTime).ToListAsync();

            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title", movieId);
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name", hallId);

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
                var movieToAdd = await _context.Movies.FindAsync(session.MovieId);
                if (movieToAdd == null) return NotFound();

                int cleaningTime = 15;
                DateTime newSessionStart = session.StartTime;
                DateTime newSessionEnd = newSessionStart.AddMinutes(movieToAdd.DurationMinutes + cleaningTime);

                var existingSessions = await _context.Sessions
                    .Include(s => s.Movie)
                    .Where(s => s.HallId == session.HallId &&
                                s.StartTime >= newSessionStart.AddDays(-1) &&
                                s.StartTime <= newSessionStart.AddDays(1))
                    .ToListAsync();

                bool hasOverlap = false;

                foreach (var existing in existingSessions)
                {
                    DateTime existingStart = existing.StartTime;
                    DateTime existingEnd = existingStart.AddMinutes(existing.Movie.DurationMinutes + cleaningTime);

                    if (newSessionStart < existingEnd && existingStart < newSessionEnd)
                    {
                        hasOverlap = true;

                        ModelState.AddModelError("StartTime",
                            $"Накладання часу! У цьому залі вже йде «{existing.Movie.Title}» з {existingStart:HH:mm} до {existingEnd:HH:mm}.");
                        break;
                    }
                }
                if (!hasOverlap)
                {
                    _context.Sessions.Add(session);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title", session.MovieId);
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name", session.HallId);
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title", session.MovieId);
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name", session.HallId);

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Session session)
        {
            if (id != session.Id) return NotFound();

            ModelState.Remove("Movie");
            ModelState.Remove("Hall");
            ModelState.Remove("Tickets");

            if (ModelState.IsValid)
            {
                var existingSession = await _context.Sessions.FindAsync(id);
                if (existingSession == null) return NotFound();

                var movieToPlay = await _context.Movies.FindAsync(session.MovieId);
                int cleaningTime = 15;
                DateTime newStart = session.StartTime;
                DateTime newEnd = newStart.AddMinutes(movieToPlay.DurationMinutes + cleaningTime);

                var overlappingSessions = await _context.Sessions
                .Include(s => s.Movie)
                .Where(s => s.HallId == session.HallId && 
                            s.Id != id &&
                            s.StartTime >= newStart.AddDays(-1) && 
                            s.StartTime <= newStart.AddDays(1))
                .ToListAsync();

                bool hasOverlap = false;
                foreach (var overlap in overlappingSessions)
                {
                    DateTime overlapStart = overlap.StartTime;
                    DateTime overlapEnd = overlapStart.AddMinutes(overlap.Movie.DurationMinutes + cleaningTime);

                    if (newStart < overlapEnd && overlapStart < newEnd)
                    {
                        hasOverlap = true;
                        ModelState.AddModelError("StartTime",
                            $"Накладання часу! У цьому залі вже йде «{overlap.Movie.Title}» з {overlapStart:HH:mm} до {overlapEnd:HH:mm}.");
                        break;
                    }
                }
                if (!hasOverlap)
                {
                    existingSession.MovieId = session.MovieId;
                    existingSession.HallId = session.HallId;
                    existingSession.StartTime = session.StartTime;
                    existingSession.Price = session.Price;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Movies = new SelectList(await _context.Movies.ToListAsync(), "Id", "Title", session.MovieId);
            ViewBag.Halls = new SelectList(await _context.Halls.ToListAsync(), "Id", "Name", session.HallId);
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
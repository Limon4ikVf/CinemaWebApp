using Cinema.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CinemaDbContext _context;

        public AdminController(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _context.Genres.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cinema.Core.Entities.Movie movie, int[] selectedGenreIds)
        {
            if (ModelState.IsValid)
            {
                if (selectedGenreIds != null && selectedGenreIds.Any())
                {
                    var selectedGenres = await _context.Genres
                        .Where(g => selectedGenreIds.Contains(g.Id))
                        .ToListAsync();

                    movie.Genres = selectedGenres;
                }

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = await _context.Genres.ToListAsync();
            return View(movie);
        }

    }
}
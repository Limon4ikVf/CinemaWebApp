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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            ViewBag.Genres = await _context.Genres.ToListAsync();
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cinema.Core.Entities.Movie updatedMovie, int[] selectedGenreIds)
        {
            if (id != updatedMovie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingMovie = await _context.Movies
                    .Include(m => m.Genres)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMovie == null) return NotFound();

                existingMovie.Title = updatedMovie.Title;
                existingMovie.Description = updatedMovie.Description;
                existingMovie.DurationMinutes = updatedMovie.DurationMinutes;
                existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
                existingMovie.PosterUrl = updatedMovie.PosterUrl;
                existingMovie.Director = updatedMovie.Director;
                existingMovie.TrailerUrl = updatedMovie.TrailerUrl;

                existingMovie.Genres.Clear();
                if (selectedGenreIds != null && selectedGenreIds.Any())
                {
                    existingMovie.Genres = await _context.Genres
                        .Where(g => selectedGenreIds.Contains(g.Id))
                        .ToListAsync();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genres = await _context.Genres.ToListAsync();
            return View(updatedMovie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
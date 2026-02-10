using System.Diagnostics;
using Cinema.Core.Entities;
using Cinema.Core.Interfaces;
using Cinema.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMovieRepository _movieRepository;

        public HomeController(ILogger<HomeController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepository.ListAllAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepository.GetMovieWithDetailsAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

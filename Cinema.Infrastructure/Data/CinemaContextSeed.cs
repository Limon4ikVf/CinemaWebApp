using Cinema.Core.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Data
{
    public static class CinemaContextSeed
    {
        public static async Task SeedAsync(CinemaDbContext context)
        {
            if (!context.Movies.Any())
            {
                var actionGenre = new Genre { Name = "Бойовик" };
                var dramaGenre = new Genre { Name = "Драма" };
                var sciFiGenre = new Genre { Name = "Фантастика" };
                var comedyGenre = new Genre { Name = "Комедія" };
                var horrorGenre = new Genre { Name = "Жахи" };

                context.Genres.AddRange(actionGenre, dramaGenre, sciFiGenre, comedyGenre, horrorGenre);

                var redHall = new Hall
                {
                    Name = "Червоний Зал",
                    Rows = 10,
                    SeatsPerRow = 15,
                };

                var blueHall = new Hall
                {
                    Name = "Синій Зал (IMAX)",
                    Rows = 12,
                    SeatsPerRow = 20,
                };

                context.Halls.AddRange(redHall, blueHall);

                var movie1 = new Movie
                {
                    Title = "Дюна: Частина друга",
                    Description = "Пол Атрід об'єднується з Чані та фрименами, щоб помститися змовникам, які знищили його родину. Перед ним стоїть вибір між коханням усього життя та порятунком всесвіту.",
                    Director = "Дені Вільнев",
                    DurationMinutes = 166,
                    ReleaseDate = new DateTime(2024, 2, 29),
                    PosterUrl = "https://image.tmdb.org/t/p/original/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                    TrailerUrl = "https://www.youtube.com/watch?v=_YUzQa_1RCE",
                    IsActive = true,
                    Genres = new List<Genre> { actionGenre, sciFiGenre, dramaGenre }
                };

                var movie2 = new Movie
                {
                    Title = "Панда Кунг-Фу 4",
                    Description = "По, Воїн Дракона, покликаний долею стати Духовним лідером Долини Миру. Це створює пару очевидних проблем...",
                    Director = "Майк Мітчелл",
                    DurationMinutes = 94,
                    ReleaseDate = new DateTime(2024, 3, 8),
                    PosterUrl = "https://image.tmdb.org/t/p/original/kDp1vUBnMpe8ak4rjgl3cLELqjU.jpg",
                    TrailerUrl = "https://www.youtube.com/watch?v=_inKs4eeHiI",
                    IsActive = true,
                    Genres = new List<Genre> { actionGenre, comedyGenre }
                };

                context.Movies.AddRange(movie1, movie2);

                await context.SaveChangesAsync();
            }

            if (!context.Sessions.Any())
            {
                var dune = await context.Movies.FirstOrDefaultAsync(m => m.Title == "Дюна: Частина друга");
                var panda = await context.Movies.FirstOrDefaultAsync(m => m.Title == "Панда Кунг-Фу 4");

                var redHall = await context.Halls.FirstOrDefaultAsync(h => h.Name == "Червоний Зал");
                var blueHall = await context.Halls.FirstOrDefaultAsync(h => h.Name == "Синій Зал (IMAX)");

                if (dune == null || panda == null || redHall == null || blueHall == null)
                {
                    return;
                }

                var sessions = new List<Session>();
                var today = DateTime.Today;

                // Сеанси для Дюни
                sessions.Add(new Session { Movie = dune, Hall = blueHall, StartTime = today.AddHours(10).AddMinutes(00), Price = 150 });
                sessions.Add(new Session { Movie = dune, Hall = blueHall, StartTime = today.AddHours(14).AddMinutes(30), Price = 180 });
                sessions.Add(new Session { Movie = dune, Hall = blueHall, StartTime = today.AddHours(19).AddMinutes(00), Price = 250 });

                // Сеанси для Панди
                sessions.Add(new Session { Movie = panda, Hall = redHall, StartTime = today.AddHours(11).AddMinutes(00), Price = 120 });
                sessions.Add(new Session { Movie = panda, Hall = redHall, StartTime = today.AddHours(15).AddMinutes(00), Price = 140 });

                // Сеанс на завтра
                sessions.Add(new Session { Movie = dune, Hall = blueHall, StartTime = today.AddDays(1).AddHours(18).AddMinutes(00), Price = 220 });

                context.Sessions.AddRange(sessions);
                await context.SaveChangesAsync();
            }
        }
    }
}
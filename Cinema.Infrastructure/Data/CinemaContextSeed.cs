using Cinema.Core.Entities;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Data
{
    public static class CinemaContextSeed
    {
        public static async Task SeedAsync(CinemaDbContext context)
        {
            if (!context.Genres.Any())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Бойовик" },
                    new Genre { Name = "Драма" },
                    new Genre { Name = "Фантастика" },
                    new Genre { Name = "Комедія" },
                    new Genre { Name = "Жахи" },
                    new Genre { Name = "Трилер" },
                    new Genre { Name = "Мелодрама" },
                    new Genre { Name = "Детектив" },
                    new Genre { Name = "Пригоди" },
                    new Genre { Name = "Фентезі" },
                    new Genre { Name = "Анімація" },
                    new Genre { Name = "Кримінал" },
                    new Genre { Name = "Сімейний" },
                    new Genre { Name = "Документальний" },
                    new Genre { Name = "Біографія" },
                    new Genre { Name = "Історичний" }
                };

                context.Genres.AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!context.Halls.Any())
            {
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
                await context.SaveChangesAsync();
            }
        }
    }
}
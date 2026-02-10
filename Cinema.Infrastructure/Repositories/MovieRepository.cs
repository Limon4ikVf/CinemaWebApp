using Cinema.Core.Entities;
using Cinema.Core.Interfaces;
using Cinema.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly CinemaDbContext _cinemaContext;

        public MovieRepository(CinemaDbContext context) : base(context)
        {
            _cinemaContext = context;
        }

        public async Task<Movie?> GetMovieWithDetailsAsync(int id)
        {
            return await _cinemaContext.Movies
                .Include(m => m.Genres)
                .Include(m => m.Sessions)
                .ThenInclude(s => s.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}

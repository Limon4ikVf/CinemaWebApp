using Cinema.Core.Entities;

namespace Cinema.Core.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie?> GetMovieWithDetailsAsync(int id);
    }
}

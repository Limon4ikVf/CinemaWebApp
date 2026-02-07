using Cinema.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування точності для грошей
            modelBuilder.Entity<Session>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            //Налаштування Many-to-Many (Фільми <-> Жанри)
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies);

            // Налаштування Many-to-Many (Фільми <-> Актори)
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies);
        }
    }
}
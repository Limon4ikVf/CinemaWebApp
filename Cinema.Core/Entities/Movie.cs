namespace Cinema.Core.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string PosterUrl { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
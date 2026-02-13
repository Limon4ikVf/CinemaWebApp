namespace Cinema.Core.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
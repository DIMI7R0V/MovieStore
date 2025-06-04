using MovieStore.Models.DTO;

namespace MovieStore.Models.Responses
{
    public class FullMovieDetails
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public DateTime DateInserted { get; set; }

        public List<Actor> ActorIds { get; set; } = new List<Actor>();
    }
}
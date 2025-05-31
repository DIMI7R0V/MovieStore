namespace MovieStore.Models.Requests
{
    public class AddMovieRequest
    {
        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }

        public List<string> Actors { get; set; }
    }
}
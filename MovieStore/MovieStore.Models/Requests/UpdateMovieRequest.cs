namespace MovieStore.Models.Requests
{
    public class UpdateMovieRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Year { get; set; }
    }
}

using MessagePack;

namespace MovieStore.Models.Requests
{
    [MessagePackObject]
    public class AddMovieRequest
    {
        [Key(1)]
        public string Title { get; set; } = string.Empty;
        
        [Key(2)]
        public int Year { get; set; }
        
        [Key(3)]
        public List<string> ActorsIds { get; set; }
    }
}
using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IMovieRepository
    {
        Task AddMovie(Movie movie);
        Task<List<Movie>> GetAllMovies();
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(string id);
        Task<Movie?> GetMovieById(string id);
    }
}
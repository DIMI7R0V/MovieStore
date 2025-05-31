using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMovies();
        Task<Movie?> GetMovieById(string id);
        Task AddMovie(Movie movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(string id);
    }
}
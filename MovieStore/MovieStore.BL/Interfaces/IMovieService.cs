using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieService 
    {
        Task AddMovie(Movie movie);
        Task<List<Movie>> GetAllMovies();
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(string id);
        Task<Movie?> GetMovieById(string id);
    }
}
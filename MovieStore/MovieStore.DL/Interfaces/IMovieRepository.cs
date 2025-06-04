using MovieStore.DL.Cache;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IMovieRepository : ICacheRepository<string, Movie>
    {
        Task<List<Movie>> GetAllMovies();
        Task AddMovie(Movie movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(string id);
        Task<Movie?> GetMovieById(string id);
    }
}
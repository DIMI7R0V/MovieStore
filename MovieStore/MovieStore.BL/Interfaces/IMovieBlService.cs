using MovieStore.Models.Responses;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieBlService
    {
        Task<List<FullMovieDetails>> GetDetailedMovies();
    }
}
using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetMovieById(int id);
        void AddMovie(Movie movie);
        void DeleteMovie(int id);
    }

}

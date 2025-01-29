using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;

        public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
        }

        public List<Movie> GetAllMovies()
        {
            return _movieRepository.GetAllMovies();
        }

        public void AddMovie(Movie? movie)
        {
            if (movie is null) return;

            foreach (var movieActor in movie.Actors)
            {
                var actor = _actorRepository.GetById(movieActor);

                if (actor is null)
                {
                    throw new Exception(
                        $"Actor with id {movieActor} does not exist");
                }
            }

            _movieRepository.AddMovie(movie);
        }

        public Movie? GetById(string id)
        {
            return _movieRepository.GetMovieById(id);
        }
    }
}

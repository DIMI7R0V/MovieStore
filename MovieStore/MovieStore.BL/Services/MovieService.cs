using Microsoft.Extensions.Logging;
using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorRepository _actorRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository, ILogger<MovieService> logger)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _logger = logger;
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMovies();
            return movies ?? new List<Movie>();
        }

        public async Task<Movie?> GetMovieById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid movie ID.");
                return null;
            }

            return await _movieRepository.GetMovieById(id);
        }

        public async Task AddMovie(Movie movie)
        {
            if (movie == null)
            {
                _logger.LogWarning("Attempted to add a null movie.");
                return;
            }

            await _movieRepository.AddMovie(movie);
            _logger.LogInformation("Movie {Title} added successfully.", movie.Title);
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrWhiteSpace(movie.Id))
            {
                _logger.LogWarning("Invalid movie data for update.");
                return false;
            }

            return await _movieRepository.UpdateMovie(movie);
        }

        public async Task<bool> DeleteMovie(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid movie ID for deletion.");
                return false;
            }

            return await _movieRepository.DeleteMovie(id);
        }
    }
}

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
        private readonly IActorBioGateway _actorBioGateway;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository, ILogger<MovieService> logger, IActorBioGateway actorBioGateway)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
            _logger = logger;
            _actorBioGateway = actorBioGateway;
        }

        //public async Task<List<Movie>> GetMovies()
        //{
        //    var test = await _actorBioGateway.GetBioByActorId("1234567890");

        //    var test1 = await _actorBioGateway.GetBioByActor(new Actor());

        //    return await _movieRepository.GetAllMovies();
        //}

        public async Task AddMovie(Movie movie)
        {
            if (movie == null || movie.ActorIds == null)
            {
                _logger.LogWarning("Attempted to add a null movie.");
                return;
            }
            movie.DateInserted = DateTime.UtcNow;
            foreach (var actor in movie.ActorIds)
            {
                if (!Guid.TryParse(actor, out _)) return;
                _logger.LogWarning("Invalid actor ID: {ActorId}", actor);
            }

            await _movieRepository.AddMovie(movie);
            _logger.LogInformation("Movie {Title} added successfully.", movie.Title);
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllMovies();
            return movies ?? new List<Movie>();
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
            var movie = await _movieRepository.GetMovieById(id);
            if (movie == null)
                return false;

            return await _movieRepository.DeleteMovie(id);
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
    }
}

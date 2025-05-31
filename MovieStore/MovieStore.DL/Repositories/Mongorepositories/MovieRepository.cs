
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movieCollection;
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<MovieRepository> logger)
        {
            _logger = logger;

            if (string.IsNullOrEmpty(mongoConfig?.CurrentValue?.ConnectionString) || string.IsNullOrEmpty(mongoConfig?.CurrentValue?.DatabaseName))
            {
                _logger.LogError("MongoDb configuration is missing");

                throw new ArgumentNullException("MongoDb configuration is missing");
            }

            var client = new MongoClient(mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(mongoConfig.CurrentValue.DatabaseName);

            _movieCollection = database.GetCollection<Movie>($"{nameof(Movie)}s");
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            try
            {
                var movies = await _movieCollection.FindAsync(movie => true);
                return await movies.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all movies.");
                return new List<Movie>();
            }
        }

        public async Task<Movie?> GetMovieById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Movie ID is null or empty.");
                return null;
            }

            try
            {
                var result = await _movieCollection.FindAsync(m => m.Id == id);
                return await result.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving movie with ID {id}.");
                return null;
            }
        }

        public async Task AddMovie(Movie movie)
        {
            if (movie == null)
            {
                _logger.LogError("Movie is null.");
                return;
            }

            try
            {
                movie.Id = Guid.NewGuid().ToString();
                await _movieCollection.InsertOneAsync(movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding movie {movie?.Title}.");
            }
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Id))
            {
                _logger.LogError("Invalid movie for update.");
                return false;
            }

            try
            {
                var result = await _movieCollection.ReplaceOneAsync(m => m.Id == movie.Id, movie);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating movie with ID {movie.Id}.");
                return false;
            }
        }

        public async Task<bool> DeleteMovie(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Invalid movie ID for deletion.");
                return false;
            }

            try
            {
                var result = await _movieCollection.DeleteOneAsync(m => m.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting movie with ID {id}.");
                return false;
            }
        }
    }
}

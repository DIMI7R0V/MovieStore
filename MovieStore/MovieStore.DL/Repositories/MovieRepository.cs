using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories
{
    internal class MovieRepository : IMovieRepository
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

        public async Task AddMovie(Movie movie)
        {
            movie.Id = Guid.NewGuid().ToString();

            await _movieCollection.InsertOneAsync(movie);
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            var result = await _movieCollection.FindAsync(_ => true);
            return await result.ToListAsync();
        }
        public async Task<bool> UpdateMovie(Movie movie)
        {
            if (movie == null || string.IsNullOrWhiteSpace(movie.Id)) return false;

            var result = await _movieCollection.ReplaceOneAsync(m => m.Id == movie.Id, movie);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteMovie(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var result = await _movieCollection.DeleteOneAsync(m => m.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<Movie?> GetMovieById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var result = await _movieCollection.FindAsync(m => m.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        protected async Task<IEnumerable<Movie?>> GetMoviesAfterDateTime(DateTime date)
        {
            var result = await _movieCollection.FindAsync(m => m.DateInserted >= date);

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Movie?>> FullLoad()
        {
            return await GetAllMovies();
        }

        public async Task<IEnumerable<Movie?>> DifLoad(DateTime lastExecuted)
        {
            return await GetMoviesAfterDateTime(lastExecuted);
        }
    }
}

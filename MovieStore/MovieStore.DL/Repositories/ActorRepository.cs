
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories
{
    internal class ActorRepository : IActorRepository
    {
        private readonly IMongoCollection<Actor> _actorsCollection;
        private readonly ILogger<ActorRepository> _logger;

        public ActorRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoConfig,
            ILogger<ActorRepository> logger)
        {
            _logger = logger;

            if (string.IsNullOrEmpty(mongoConfig?.CurrentValue?.ConnectionString) || string.IsNullOrEmpty(mongoConfig?.CurrentValue?.DatabaseName))
            {
                _logger.LogError("MongoDb configuration is missing");

                throw new ArgumentNullException("MongoDb configuration is missing");
            }

            var client = new MongoClient(mongoConfig.CurrentValue.ConnectionString);

            var database = client.GetDatabase(mongoConfig.CurrentValue.DatabaseName);

            _actorsCollection = database.GetCollection<Actor>($"{nameof(Actor)}s");
        }

        public async Task AddActor(Actor actor)
        {
            actor.Id = Guid.NewGuid().ToString();
            actor.DateInserted = DateTime.UtcNow;
            await _actorsCollection.InsertOneAsync(actor);
        }

        public async Task<List<Actor>> GetAllActors()
        {
            var result = await _actorsCollection.FindAsync(m => true);

            return await result.ToListAsync();
        }

        public async Task<bool> UpdateActor(Actor actor)
        {
            var result = await _actorsCollection.ReplaceOneAsync(a => a.Id == actor.Id, actor);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteActor(string id)
        {
            var result = await _actorsCollection.DeleteOneAsync(a => a.Id == id);

            return result.DeletedCount > 0;
        }

        public async Task<Actor?> GetActorById(string id)
        {
            var result = await _actorsCollection.FindAsync(a => a.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Actor?>> DifLoad(DateTime lastExecuted)
        {
            var result = await _actorsCollection.FindAsync(m => m.DateInserted >= lastExecuted);

            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Actor?>> FullLoad()
        {
            return await GetAllActors();
        }
    }
}

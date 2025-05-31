
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Configurations;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Repositories.MongoRepositories
{
    public class ActorRepository : IActorRepository
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
            if (actor == null)
            {
                _logger.LogError("Attempted to add a null actor.");
                return;
            }

            try
            {
                actor.Id = Guid.NewGuid().ToString();
                await _actorsCollection.InsertOneAsync(actor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding actor {actor?.Name}.");
            }
        }

        public async Task<IEnumerable<Actor>> GetActorsByIds(IEnumerable<string> actorIds)
        {
            try
            {
                var filter = Builders<Actor>.Filter.In(a => a.Id, actorIds);
                var result = await _actorsCollection.FindAsync(filter);
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving actors by IDs.");
                return new List<Actor>();
            }
        }

        public async Task<Actor?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid actor ID provided.");
                return null;
            }

            try
            {
                var result = await _actorsCollection.FindAsync(a => a.Id == id);
                return await result.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving actor with ID {id}.");
                return null;
            }
        }

        public async Task<List<Actor>> GetAll()
        {
            try
            {
                var result = await _actorsCollection.FindAsync(_ => true);
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all actors.");
                return new List<Actor>();
            }
        }

        public async Task<bool> UpdateActor(Actor actor)
        {
            if (actor == null || string.IsNullOrWhiteSpace(actor.Id))
            {
                _logger.LogError("Invalid actor data for update.");
                return false;
            }

            try
            {
                var filter = Builders<Actor>.Filter.Eq(a => a.Id, actor.Id);
                var result = await _actorsCollection.ReplaceOneAsync(filter, actor);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating actor.");
                return false;
            }
        }

        public async Task<bool> DeleteActor(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogError("Invalid ID for deletion.");
                return false;
            }

            try
            {
                var filter = Builders<Actor>.Filter.Eq(a => a.Id, id);
                var result = await _actorsCollection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting actor with ID {id}.");
                return false;
            }
        }

    }
}

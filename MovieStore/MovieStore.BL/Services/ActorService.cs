using Microsoft.Extensions.Logging;
using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    internal class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly ILogger<ActorService> _logger;

        public ActorService(IActorRepository actorRepository, ILogger<ActorService> logger)
        {
            _actorRepository = actorRepository;
            _logger = logger;
        }

        public async Task AddActor(Actor actor)
        {
            if (actor == null)
            {
                _logger.LogWarning("Attempted to add a null actor.");
                return;
            }

            await _actorRepository.AddActor(actor);
            _logger.LogInformation("Actor {ActorName} added successfully.", actor.Name);
        }

        public async Task<IEnumerable<Actor>> GetActorsByIds(IEnumerable<string> actorIds)
        {
            if (actorIds == null || !actorIds.Any())
            {
                _logger.LogWarning("Actor ID list is null or empty.");
                return Enumerable.Empty<Actor>();
            }

            return await _actorRepository.GetActorsByIds(actorIds);
        }

        public async Task<Actor?> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid actor ID.");
                return null;
            }

            return await _actorRepository.GetById(id);
        }

        public async Task<List<Actor>> GetAll()
        {
            return await _actorRepository.GetAll();
        }

        public async Task<bool> UpdateActor(Actor actor)
        {
            if (actor == null || string.IsNullOrWhiteSpace(actor.Id))
            {
                _logger.LogWarning("Invalid actor for update.");
                return false;
            }

            return await _actorRepository.UpdateActor(actor);
        }

        public async Task<bool> DeleteActor(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid actor ID for deletion.");
                return false;
            }

            return await _actorRepository.DeleteActor(id);
        }
    }
}

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
        public async Task<List<Actor>> GetAllActors()
        {
            return await _actorRepository.GetAllActors();
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

        public async Task<Actor?> GetActorById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid actor ID.");
                return null;
            }

            return await _actorRepository.GetActorById(id);
        }


    }
}

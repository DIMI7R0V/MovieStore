using Microsoft.Extensions.Logging;
using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.BL.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActorBioGateway _actorBioGateway;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<ActorService> _logger;

        public ActorService(IActorRepository actorRepository, ILogger<ActorService> logger, IActorBioGateway actorBioGateway, IMovieRepository movieRepository)
        {
            _actorRepository = actorRepository;
            _logger = logger;
            _actorBioGateway = actorBioGateway;
            _movieRepository = movieRepository;
        }

        //public async Task<List<Movie>> GetMovies()
        //{
        //    var test = await _actorBioGateway.GetBioByActorId("1234567890");

        //    var test1 = await _actorBioGateway.GetBioByActor(new Actor());
        //    return await _movieRepository.GetAllMovies();
        //}

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

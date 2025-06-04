using MovieStore.DL.Cache;
using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IActorRepository : ICacheRepository<string, Actor>
    {
        Task AddActor(Actor actor);
        Task<List<Actor>> GetAllActors();
        Task<bool> UpdateActor(Actor actor);
        Task<bool> DeleteActor(string id);
        Task<Actor?> GetActorById(string id);
    }
}
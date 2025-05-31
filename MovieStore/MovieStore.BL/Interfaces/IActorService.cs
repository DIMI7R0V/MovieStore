using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IActorService
    {
        Task AddActor(Actor actor);
        Task<IEnumerable<Actor>> GetActorsByIds(IEnumerable<string> actorIds);
        Task<Actor?> GetById(string id);
        Task<List<Actor>> GetAll();
        Task<bool> UpdateActor(Actor actor);
        Task<bool> DeleteActor(string id);
    }
}
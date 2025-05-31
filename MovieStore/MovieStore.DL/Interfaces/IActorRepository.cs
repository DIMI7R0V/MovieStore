using MovieStore.Models.DTO;

namespace MovieStore.DL.Interfaces
{
    public interface IActorRepository
    {
        Task AddActor(Actor actor);
        Task<IEnumerable<Actor>> GetActorsByIds(IEnumerable<string> actorsIds);
        Task<Actor?> GetById(string id);
        Task<List<Actor>> GetAll();
        Task<bool> UpdateActor(Actor actor);
        Task<bool> DeleteActor(string id);
    }
}
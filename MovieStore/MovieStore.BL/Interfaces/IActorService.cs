using MovieStore.Models.DTO;

namespace MovieStore.BL.Interfaces
{
    public interface IActorService
    {
        Task AddActor(Actor actor);
        Task<List<Actor?>> GetAllActors();
        Task<bool> UpdateActor(Actor actor);
        Task<bool> DeleteActor(string id);
        Task<Actor?> GetActorById(string id);
    }
}
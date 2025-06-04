using MovieStore.Models.DTO;
using MovieStore.Models.Responses;

namespace MovieStore.DL.Interfaces
{
    public interface IActorBioGateway
    {
        Task<ActorBioResponse> GetBioByActorId(string actorId);

        Task<ActorBioResponse> GetBioByActor(Actor actorId);
    }
}

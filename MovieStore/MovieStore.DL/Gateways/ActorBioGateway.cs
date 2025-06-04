using MongoDB.Bson.IO;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Responses;
using RestSharp;
using System.Text;

namespace MovieStore.DL.Gateways
{
    internal class ActorBioGateway : IActorBioGateway
    {
        private readonly RestClient _client;

        public ActorBioGateway()
        {
            var options = new RestClientOptions("https://localhost:7218");

            _client = new RestClient(options);

            // The cancellation token comes from the caller. You can still make a call without it.
        }

        public async Task<ActorBioResponse> GetBioByActor(Actor actor)
        {
            var request = new RestRequest($"/ActorData", Method.Post);

            request.AddJsonBody(actor);

            var response = await _client.ExecuteAsync<ActorBioResponse>(request);

            return response.Data;
        }

        public async Task<ActorBioResponse> GetBioByActorId(string actorId)
        {
            var request = new RestRequest($"/ActorData", Method.Get);

            request.AddQueryParameter("actorId", actorId);

            var response = await _client.ExecuteAsync<ActorBioResponse>(request);

            return response.Data;
        }
    }
}

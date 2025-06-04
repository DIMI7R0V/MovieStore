using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Responses;

namespace MovieStore.BL.Services
{
    public class MovieBLService : IMovieBlService
    {
        
        private readonly IMovieService _movieService;
        private readonly IActorService _actorService;

        public MovieBLService(IActorService actorService, IMovieService movieService)
        {
            _actorService = actorService;
            _movieService = movieService;
        }

        public async Task<List<FullMovieDetails>> GetDetailedMovies()
        {
            var result = new List<FullMovieDetails>();
            var movies = await _movieService.GetAllMovies();

            foreach (var movie in movies)
            {
                var movieDetails = new FullMovieDetails
                {
                    Title = movie.Title,
                    Year = movie.Year,
                    Id = movie.Id,
                    DateInserted = movie.DateInserted,
                    ActorIds = new List<Actor>() // Ensure FullMovieDetails has this property
                };

                foreach (var actorId in movie.ActorIds)
                {
                    var actor = await _actorService.GetActorById(actorId);
                    if (actor != null)
                    {
                        movieDetails.ActorIds.Add(actor);
                    }
                }

                result.Add(movieDetails);
            }

            return result;
        }

    }
}

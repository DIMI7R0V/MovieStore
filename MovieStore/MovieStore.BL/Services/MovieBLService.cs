using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Responses;

namespace MovieStore.BL.Services
{
    internal class MovieBLService : IMovieBlService
    {
        private readonly IMovieService _movieService;
        private readonly IActorRepository _actorRepository;

        public MovieBLService(IMovieService movieRepository, IActorRepository actorRepository)
        {
            _movieService = movieRepository;
            _actorRepository = actorRepository;
        }

        public async Task<List<FullMovieDetails>> GetDetailedMovies()
        {
            var result = new List<FullMovieDetails>();

            var movies = await _movieService.GetAllMovies();

            foreach (var movie in movies)
            {
                var movieDetails = new FullMovieDetails();
                movieDetails.Title = movie.Title;
                movieDetails.Year = movie.Year;
                movieDetails.Id = movie.Id;

                foreach (var actorId in movie.ActorIds)
                {
                    var actor = _actorRepository.GetActorById(actorId);
                }

                result.Add(movieDetails);
            }
            return result;
        }

    }
}

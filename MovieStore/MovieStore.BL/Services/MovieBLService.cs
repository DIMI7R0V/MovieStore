using MovieStore.BL.Interfaces;
using MovieStore.DL.Interfaces;
using MovieStore.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.BL.Services
{
    internal class MovieBLService : IMovieBLService
    {
        private readonly IMovieService _movieRepository;
        private readonly IActorRepository _actorRepository;

        public MovieBLService(IMovieService movieRepository, IActorRepository actorRepository)
        {
            _movieRepository = movieRepository;
            _actorRepository = actorRepository;
        }

        public List<MovieView> GetDetailedMovies()
        {
            var result  = new List<MovieView>();

            var movies = _movieRepository.GetAllMovies();

            foreach (var movie in movies)
            {
                var actors = new List<Actor>();
                var movieView = new MovieView()
                {
                    MovieId = movie.Id,
                    MovieTitle = movie.Title,
                    MovieYear = movie.Year
                    Actors = _actorRepository.GetActorsById(movie.Id)
                };
                

                foreach (var actor in movie.Actors)
                {
                    var actorDto = _actorRepository.GetActorById(actor.Id);
                    movieView.Actors.Add(actorDto);
                }
                movieView.Actors = movieView.
                result.Add(movieView);
            }
        }

        return result;
    }
}

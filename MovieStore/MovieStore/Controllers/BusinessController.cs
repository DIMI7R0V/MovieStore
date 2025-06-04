
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.BL.Services;
using MovieStore.Models.DTO;
using MovieStore.Models.Responses;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IMovieBlService _movieBlService;
        private readonly ILogger<BusinessController> _logger;
        //private readonly MovieLookupService _movieLookupService;

        public BusinessController(
            IMovieBlService movieBlService,
            ILogger<BusinessController> logger
            //MovieLookupService movieLookupService
            )
        {
            _movieBlService = movieBlService;
            _logger = logger;
            //_movieLookupService = movieLookupService;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetDetailedMovies")]
        public async Task<IEnumerable<FullMovieDetails>> GetDetailedMovies()
        {
            return await _movieBlService.GetDetailedMovies();

        }
    }
}

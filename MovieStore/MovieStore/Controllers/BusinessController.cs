
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Responses;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IActorService _actorService;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(
            IMovieService movieService,
            IActorService actorService,
            ILogger<BusinessController> logger)
        {
            _movieService = movieService;
            _actorService = actorService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetDetailedMovies")]
        public async Task<IActionResult> GetDetailedMovies()
        {
            var result = await _movieService.GetAllMovies();

            if (result == null || !result.Any())
            {
                return NotFound("No products found!");
            }

            return Ok(result);
        }

        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpPost("AddActor")]
        //public async Task<IActionResult> AddActor([FromBody] Actor actor)
        //{
        //    if (actor == null)
        //    {
        //        return BadRequest("Actor cannot be null.");
        //    }

        //    try
        //    {
        //        await _actorService.AddActor(actor);
        //        return Ok("Actor added successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to add actor.");
        //        return BadRequest("Error adding actor.");
        //    }
        //}


    }
}

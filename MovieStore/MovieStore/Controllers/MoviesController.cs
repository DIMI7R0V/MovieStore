
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieController> _logger;

        public MovieController(
            IMovieService movieService,
            IMapper mapper,
            ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Movie request)
        {
            var movie = _mapper.Map<Movie>(request);

            await _movieService.AddMovie(movie);

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> Get()
        {
            var result = await _movieService.GetAllMovies();
            return result.Any() ? Ok(result) : NotFound("No movies found");
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] Movie movie)
        {
            var updated = await _movieService.UpdateMovie(movie);
            if (!updated)
                return NotFound($"Movie with ID:{movie.Id} not found");

            return NoContent();
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id must not be empty");

            var deleted = await _movieService.DeleteMovie(id);
            return deleted ? NoContent() : NotFound($"Movie with ID:{id} not found");
        }

        [HttpGet("GetMovieById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id can't be null or empty");

            var result = await _movieService.GetMovieById(id);
            return result == null ? NotFound($"Movie with ID: {id} not found") : Ok(result);
        }
    }
}

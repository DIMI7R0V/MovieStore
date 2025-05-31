
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieControler : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieControler> _logger;

        public MovieControler(
            IMovieService movieService,
            IMapper mapper,
            ILogger<MovieControler> logger)
        {
            _movieService = movieService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AddMovieRequest request)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);

                if (movie == null)
                    return BadRequest("Invalid movie data");

                await _movieService.AddMovie(movie);

                return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding movie");
                return BadRequest("Something went wrong.");
            }
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> Get()
        {
            var result = await _movieService.GetAllMovies();

            if (result == null || !result.Any())
                return NotFound("No movies found");

            return Ok(result);
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
            if (!deleted)
                return NotFound($"Movie with ID:{id} not found");

            return NoContent();
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

            if (result == null)
                return NotFound($"Movie with ID:{id} not found");

            return Ok(result);
        }
    }
}

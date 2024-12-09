using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;
using MovieStore.Models.Requests;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieBLControler : ControllerBase
    {
        private readonly IMovieBLService _movieService;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieBLControler> _logger;

        public MovieBLControler(
            IMovieBLService movieBLService,
            IMapper mapper,
            ILogger<MovieBLControler> logger)
        {
            _movieService = movieBLService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting movie with ID: {id}");
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0");
            }
            var result = _movieService.GetById(id);

            if (result == null)
            {
                return NotFound($"Movie with ID:{id} not found");
            }

            return Ok(result);
        }

    }
}
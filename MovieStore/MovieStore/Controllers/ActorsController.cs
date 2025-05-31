using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MovieStore.BL.Interfaces;
using MovieStore.Models.DTO;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        private readonly IMapper _mapper;
        private readonly ILogger<ActorController> _logger;

        public ActorController(
            IActorService actorService,
            IMapper mapper,
            ILogger<ActorController> logger)
        {
            _actorService = actorService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _actorService.GetAll();

            if (result == null || !result.Any())
                return NotFound("No actors found.");

            return Ok(result);
        }

        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be null or empty.");

            var actor = await _actorService.GetById(id);

            if (actor == null)
                return NotFound($"Actor with ID: {id} not found.");

            return Ok(actor);
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Actor request)
        {
            try
            {
                var actor = _mapper.Map<Actor>(request);

                if (actor == null)
                    return BadRequest("Invalid actor data.");

                await _actorService.AddActor(actor);

                return CreatedAtAction(nameof(GetById), new { id = actor.Id }, actor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding actor.");
                return BadRequest("Something went wrong.");
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] Actor actor)
        {
            var updated = await _actorService.UpdateActor(actor);
            if (!updated)
                return NotFound($"Actor with ID: {actor.Id} not found.");

            return NoContent();
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Id cannot be empty.");

            var deleted = await _actorService.DeleteActor(id);
            if (!deleted)
                return NotFound($"Actor with ID: {id} not found.");

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interfaces;
using StudentManagement.ViewModels;

namespace StudentManagement.Controllers.Api
{
    [ApiController]
    [Route("api/flights")]
    [Produces("application/json")]
    public class FlightsApiController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsApiController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetAll()
        {
            var flights = await _flightService.GetAllAsync();
            return Ok(flights);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FlightDto>> GetById(Guid id)
        {
            var flight = await _flightService.GetByIdAsync(id);
            if (flight is null)
            {
                return NotFound(new { message = $"Flight with id {id} was not found." });
            }

            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<FlightDto>> Create([FromBody] FlightCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, created, error) = await _flightService.CreateAsync(dto);
            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return CreatedAtAction(nameof(GetById), new { id = created!.FlightId }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FlightDto>> Update(Guid id, [FromBody] FlightCreateUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, updated, error) = await _flightService.UpdateAsync(id, dto);
            if (!success)
            {
                if (error == "Flight not found.")
                {
                    return NotFound(new { message = error });
                }

                return BadRequest(new { message = error });
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _flightService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Flight with id {id} was not found." });
            }

            return NoContent();
        }
    }
}

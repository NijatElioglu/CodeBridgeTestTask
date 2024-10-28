using CodeBridgeTestTask.Application.Features.Dog.Commands.Create;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Delete;
using CodeBridgeTestTask.Application.Features.Dog.Commands.Update;
using CodeBridgeTestTask.Application.Features.Dog.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeBridgeTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DogController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public IActionResult GetPing()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }

        [HttpPost("CreateDog")]
        public async Task<IActionResult> CreateDog([FromBody] CreateDogCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var dogId = await _mediator.Send(command);
                return CreatedAtAction(nameof(CreateDog), new { id = dogId }, command);
            }
            catch (DbUpdateException dbEx)
            {

                return StatusCode(500, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






        [HttpGet("GetAllDogs")]   

        public async Task<IActionResult> GetAllDogs([FromQuery] string attribute = "name", [FromQuery] string order = "asc", [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetDogsQuery
            {
                Attribute = attribute,
                Order = order,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("UpdateDog")]
        public async Task<IActionResult> UpdateDog( [FromBody] UpdateDogCommand command)
        {
          
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the dog.");
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> SoftDeleteDog(int dogId)
        {
            var command = new SoftDeleteDogCommand { DogId = dogId };
            try
            {
                await _mediator.Send(command);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error occurred while soft deleting the dog: {ex.Message}");
            }
        }
    }
}

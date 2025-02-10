using API.BusinessLogic;
using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : BaseApiController<Rental, RentalDto, RentalDto>
    {
        private readonly RentalsService _service;
        private readonly IRentalProcessing _rentalProcessing;

        public RentalsController(RentalsService service, IRentalProcessing rentalProcessing) : base(service)
        {
            _service = service;
            _rentalProcessing = rentalProcessing;
        }

        protected override int GetEntityId(RentalDto entity)
        {
            return entity.RentalId;
        }

        [HttpGet("awaiting")]
        public async Task<IActionResult> GetAwaitingRentals(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            try
            {
                var paginatedResult = await _service.GetAwaitingRentalsAsync(
                    search, page, pageSize, createdBefore, createdAfter, modifiedBefore, modifiedAfter);

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving awaiting to pickup rentals.");
            }
        }

        [HttpPut("mark-pickup")]
        public async Task<IActionResult> MarkPickup(RentalDto rental)
        {
            try
            {
                // Mark the rental as picked up
                await _rentalProcessing.MarkPickupAsync(rental);
                return Ok(rental);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while marking a rent.");
            }
        }

        [HttpGet("inprogress")]
        public async Task<IActionResult> GetInProgressRentals(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            try
            {
                var paginatedResult = await _service.GetInProgressRentalsAsync(
                    search, page, pageSize, createdBefore, createdAfter, modifiedBefore, modifiedAfter);

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving in progress rentals.");
            }
        }

        [HttpPut("mark-return")]
        public async Task<IActionResult> MarkReturn(RentalDto rental)
        {
            try
            {
                // Mark the rental as returned
                await _rentalProcessing.MarkReturnAsync(rental);
                return Ok(rental);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while marking a rent.");
            }
        }
    }
}

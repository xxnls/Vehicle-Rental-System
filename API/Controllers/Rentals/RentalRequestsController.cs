using API.BusinessLogic;
using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalRequestsController : BaseApiController<RentalRequest, RentalRequestDto, RentalRequestDto>
    {
        private readonly RentalRequestsService _service;
        private readonly IRentalProcessing _rentalProcessing;
        public RentalRequestsController(RentalRequestsService service, IRentalProcessing rentalProcessing) : base(service)
        {
            _service = service;
            _rentalProcessing = rentalProcessing;
        }

        protected override int GetEntityId(RentalRequestDto entity)
        {
            return entity.RentalRequestId;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests(
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
                var paginatedResult = await _service.GetPendingRequestsAsync(
                    search, page, pageSize, createdBefore, createdAfter, modifiedBefore, modifiedAfter);

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving pending requests.");
            }
        }

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRentalRequest(RentalRequestDto rentalRequest)
        {
            try
            {
                var rentalDto = await _rentalProcessing.ApproveRentalRequestAsync(rentalRequest);
                return Ok(rentalDto);
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
                return StatusCode(500, "An error occurred while approving the request.");
            }
        }
    }
}

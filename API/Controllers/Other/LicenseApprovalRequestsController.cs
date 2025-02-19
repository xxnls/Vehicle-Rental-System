using API.Models.DTOs.Other;
using API.Models.Other;
using API.Services.Other;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseApprovalRequestsController : BaseApiController<LicenseApprovalRequests, LicenseApprovalRequestsDto, LicenseApprovalRequestsDto>
    {
        private readonly LicenseApprovalRequestsService _service;

        public LicenseApprovalRequestsController(LicenseApprovalRequestsService service) : base(service)
        {
            _service = service;
        }

        protected override int GetEntityId(LicenseApprovalRequestsDto entity)
        {
            return entity.LicenseApprovalRequestId;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLicenseApprovalRequests(
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
                var paginatedResult = await _service.GetPendingLicenseApprovalRequestsAsync(
                    search, page, pageSize, createdBefore, createdAfter, modifiedBefore, modifiedAfter);
                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving pending license approval requests.");
            }
        }
    }
}

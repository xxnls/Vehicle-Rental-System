using API.BusinessLogic;
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
        private readonly ILicenseProcessing _licenseProcessing;

        public LicenseApprovalRequestsController(LicenseApprovalRequestsService service, ILicenseProcessing licenseProcessing) : base(service)
        {
            _service = service;
            _licenseProcessing = licenseProcessing;
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

        [HttpPut("approve/{requestId}/{employeeId}")]
        public async Task<IActionResult> ApproveLicense(int requestId, int employeeid)
        {
            try
            {
                await _licenseProcessing.ApproveLicenseAsync(requestId, employeeid);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while approving license.");
            }
        }

        [HttpPost("upload-license")]
        public async Task<IActionResult> UploadLicense(UploadLicenseDto request)
        {
            try
            {
                await _licenseProcessing.UploadLicense(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading license.");
            }
        }

    }
}

using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.PostRentalReports;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostRentalReportsController(PostRentalReportsService service)
        : BaseApiController<PostRentalReport, PostRentalReportDto, PostRentalReportDto>(service)
    {

        protected override int GetEntityId(PostRentalReportDto entity)
        {
            return entity.PostRentalReportId;
        }
    }
}
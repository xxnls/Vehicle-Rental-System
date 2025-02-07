using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalRequestsController(RentalRequestsService service) : BaseApiController<RentalRequest, RentalRequestDto, RentalRequestDto>(service)
    {
        protected override int GetEntityId(RentalRequestDto entity)
        {
            return entity.RentalRequestId;
        }
    }
}

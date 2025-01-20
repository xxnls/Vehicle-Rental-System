using API.Models.DTOs.Other;
using API.Models.Other;
using API.Services.Other;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalPlacesController(RentalPlacesService service) : BaseApiController<RentalPlace, RentalPlaceDto, RentalPlaceDto>(service)
    {
        protected override int GetEntityId(RentalPlaceDto entity)
        {
            return entity.RentalPlaceId;
        }
    }
}

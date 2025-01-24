using Microsoft.AspNetCore.Mvc;
using API.Models.DTOs.Other;
using API.Services.Other;
using API.Models.Other;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController(LocationsService service) : BaseApiController<Location, LocationDto, LocationDto>(service)
    {
        protected override int GetEntityId(LocationDto entity)
        {
            return entity.LocationId;
        }
    }
}

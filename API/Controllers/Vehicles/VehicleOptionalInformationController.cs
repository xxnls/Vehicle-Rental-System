using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleOptionalInformationController(VehicleOptionalInformationService service)
        : BaseApiController<VehicleOptionalInformation, VehicleOptionalInformationDto, VehicleOptionalInformationDto>(
            service)
    {
        protected override int GetEntityId(VehicleOptionalInformationDto entity)
        {
            return entity.VehicleOptionalInformationId;
        }
    }
}

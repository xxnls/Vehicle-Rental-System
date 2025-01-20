using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController(VehicleTypesService service) : BaseApiController<VehicleType, VehicleTypeDto, VehicleTypeDto>(service)
    {
        protected override int GetEntityId(VehicleTypeDto entity)
        {
            return entity.VehicleTypeId;
        }
    }
}

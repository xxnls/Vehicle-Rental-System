using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleStatusesController(VehicleStatusesService service)
        : BaseApiController<VehicleStatus, VehicleStatusDto, VehicleStatusDto>(service)
    {
        protected override int GetEntityId(VehicleStatusDto entity)
        {
            return entity.VehicleStatusId;
        }
    }
}
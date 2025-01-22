using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleStatisticsController(VehicleStatisticsService service)
        : BaseApiController<VehicleStatistic, VehicleStatisticsDto, VehicleStatisticsDto>(service)
    {
        protected override int GetEntityId(VehicleStatisticsDto entity)
        {
            return entity.VehicleStatisticsId;
        }
    }
}

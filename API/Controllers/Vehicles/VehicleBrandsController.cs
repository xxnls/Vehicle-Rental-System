using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.DTOs.Vehicles;
using API.Services;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBrandsController(VehicleBrandsService service) : BaseApiController<VehicleBrand, VehicleBrandDto, VehicleBrandDto>(service)
    {
        protected override int GetEntityId(VehicleBrandDto entity)
        {
            return entity.VehicleBrandId;
        }
    }
}

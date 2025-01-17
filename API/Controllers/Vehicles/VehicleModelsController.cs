using API.Models;
using API.Models.DTOs.Vehicles;
using API.Services.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Vehicles
{
    public class VehicleModelsController(VehicleModelsService service) : BaseApiController<VehicleModel, VehicleModelDto, VehicleModelDto>(service)
    {
        protected override int GetEntityId(VehicleModelDto entity)
        {
            return entity.VehicleModelId;
        }
    }
}

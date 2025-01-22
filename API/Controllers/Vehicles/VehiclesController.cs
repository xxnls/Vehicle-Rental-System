using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models.DTOs;
using API.Models.Vehicles;
using API.Models.DTOs.Vehicles;
using API.Services.Vehicles;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController(VehiclesService service)
        : BaseApiController<Vehicle, VehicleDto, VehicleDto>(service)
    {
        protected override int GetEntityId(VehicleDto entity)
        {
            return entity.VehicleId;
        }
    }
}

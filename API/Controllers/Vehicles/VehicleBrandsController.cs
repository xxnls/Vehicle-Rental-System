using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models;
using API.Models.DTOs;
using API.Services;
using Azure;

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

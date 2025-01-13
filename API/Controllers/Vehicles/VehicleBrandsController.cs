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
    public class VehicleBrandsController : BaseApiController<VehicleBrand, VehicleBrandDto, VehicleBrandDto>
    {
        public VehicleBrandsController(VehicleBrandsService service) : base(service)
        {
        }

        protected override int GetEntityId(VehicleBrandDto entity)
        {
            return entity.VehicleBrandId;
        }
    }
}

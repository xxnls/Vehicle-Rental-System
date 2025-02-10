using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
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
    public class VehiclesController : BaseApiController<Vehicle, VehicleDto, VehicleDto>
    {
        private readonly VehiclesService _service;
        private readonly IRentalProcessing _rentalProcessing;
        public VehiclesController(VehiclesService service, IRentalProcessing rentalProcessing) : base(service)
        {
            _service = service;
            _rentalProcessing = rentalProcessing;
        }

        protected override int GetEntityId(VehicleDto entity)
        {
            return entity.VehicleId;
        }

        [HttpGet("maintenance")]
        public async Task<IActionResult> GetVehiclesMaintenance(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            try
            {
                var paginatedResult = await _service.GetVehiclesMaintenanceAsync(
                    search, page, pageSize, createdBefore, createdAfter, modifiedBefore, modifiedAfter);

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving vehicles with \"Maintenance\" status.");
            }
        }

        [HttpPut("status/{id}/{statusId}")]
        public async Task<IActionResult> ChangeVehicleStatus(int id, int statusId)
        {
            try
            {
                await _rentalProcessing.ChangeVehicleStatusAsync(id, statusId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while changing vehicle status.");
            }
        }
    }
}

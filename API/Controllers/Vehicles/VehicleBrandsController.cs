using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models.Vehicles.VehicleBrands;
using API.Models.Vehicles.VehicleBrands.DTOs;

namespace API.Controllers.Vehicles
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBrandsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public VehicleBrandsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleBrands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RVehicleBrandDTO>>> GetVehicleBrands()
        {
            var vehicleBrandDTOs = await _context.VehicleBrands
                .Where(vb => vb.IsActive)
                .Select(vb => new RVehicleBrandDTO
                {
                    VehicleBrandId = vb.VehicleBrandId,
                    Name = vb.Name,
                    Description = vb.Description,
                    Website = vb.Website,
                    LogoUrl = vb.LogoUrl,
                    CreatedDate = vb.CreatedDate,
                    ModifiedDate = vb.ModifiedDate,
                    DeletedDate = vb.DeletedDate,
                    IsActive = vb.IsActive
                })
                .ToListAsync();

            return Ok(vehicleBrandDTOs);
        }

        // GET: api/VehicleBrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RVehicleBrandDTO>> GetVehicleBrand(int id)
        {
            var vehicleBrand = await _context.VehicleBrands.FindAsync(id);

            if (vehicleBrand == null)
            {
                return NotFound();
            }

            var vehicleBrandDTO = new RVehicleBrandDTO
            {
                VehicleBrandId = vehicleBrand.VehicleBrandId,
                Name = vehicleBrand.Name,
                Description = vehicleBrand.Description,
                Website = vehicleBrand.Website,
                LogoUrl = vehicleBrand.LogoUrl,
                CreatedDate = vehicleBrand.CreatedDate,
                ModifiedDate = vehicleBrand.ModifiedDate,
                DeletedDate = vehicleBrand.DeletedDate,
                IsActive = vehicleBrand.IsActive
            };

            return Ok(vehicleBrandDTO);
        }

        // PUT: api/VehicleBrands/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleBrand(int id, CUVehicleBrandDTO vehicleBrandDto)
        {
            var existingVehicleBrand = await _context.VehicleBrands.FindAsync(id);
            if (existingVehicleBrand == null)
            {
                return NotFound();
            }

            existingVehicleBrand.Name = vehicleBrandDto.Name;
            existingVehicleBrand.Description = vehicleBrandDto.Description;
            existingVehicleBrand.Website = vehicleBrandDto.Website;
            existingVehicleBrand.LogoUrl = vehicleBrandDto.LogoUrl;
            existingVehicleBrand.ModifiedDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleBrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VehicleBrands
        [HttpPost]
        public async Task<ActionResult<RVehicleBrandDTO>> PostVehicleBrand(CUVehicleBrandDTO vehicleBrandDto)
        {
            var newVehicleBrand = new VehicleBrand
            {
                Name = vehicleBrandDto.Name,
                Description = vehicleBrandDto.Description,
                Website = vehicleBrandDto.Website,
                LogoUrl = vehicleBrandDto.LogoUrl,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.VehicleBrands.Add(newVehicleBrand);
            await _context.SaveChangesAsync();

            var responseDto = new RVehicleBrandDTO
            {
                VehicleBrandId = newVehicleBrand.VehicleBrandId,
                Name = newVehicleBrand.Name,
                Description = newVehicleBrand.Description,
                Website = newVehicleBrand.Website,
                LogoUrl = newVehicleBrand.LogoUrl,
                CreatedDate = newVehicleBrand.CreatedDate,
                IsActive = newVehicleBrand.IsActive
            };

            return CreatedAtAction(nameof(GetVehicleBrand), new { id = newVehicleBrand.VehicleBrandId }, responseDto);
        }

        // DELETE: api/VehicleBrands/5
        // SOFT DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleBrand(int id)
        {
            var vehicleBrand = await _context.VehicleBrands.FindAsync(id);
            if (vehicleBrand == null)
            {
                return NotFound();
            }

            vehicleBrand.DeletedDate = DateTime.UtcNow;
            vehicleBrand.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleBrandExists(int id)
        {
            return _context.VehicleBrands.Any(e => e.VehicleBrandId == id);
        }
    }
}

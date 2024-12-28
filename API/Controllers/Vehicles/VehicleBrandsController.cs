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
using Azure;

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
        // GET: api/VehicleBrands?search=...
        // GET: api/VehicleBrands?search=...&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<RVehicleBrandDTO>>> GetVehicleBrands(string? search = null, int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            IQueryable<VehicleBrand> query = _context.VehicleBrands.Where(vb => vb.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(vb =>
                    vb.Name.Contains(search) ||
                    (vb.Description != null && vb.Description.Contains(search)) ||
                    (vb.Website != null && vb.Website.Contains(search)) ||
                    (vb.LogoUrl != null && vb.LogoUrl.Contains(search))
                );
            }

            // Get total count for pagination metadata
            int totalItemCount = await query.CountAsync();

            // Apply pagination
            var vehicleBrandDTOs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            // Return paginated result
            var result = new PaginatedResult<RVehicleBrandDTO>
            {
                Items = vehicleBrandDTOs,
                TotalItemCount = totalItemCount,
                CurrentPage = page,
                PageSize = pageSize
            };

            return Ok(result);
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

            var vehicleBrandDTOs = new RVehicleBrandDTO
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

            return Ok(vehicleBrandDTOs);
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

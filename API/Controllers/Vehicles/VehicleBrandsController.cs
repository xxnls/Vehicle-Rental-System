using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models.Vehicles;

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
        public async Task<ActionResult<IEnumerable<VehicleBrand>>> GetVehicleBrands()
        {
            return await _context.VehicleBrands.ToListAsync();
        }

        // GET: api/VehicleBrands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleBrand>> GetVehicleBrand(int id)
        {
            var vehicleBrand = await _context.VehicleBrands.FindAsync(id);

            if (vehicleBrand == null)
            {
                return NotFound();
            }

            return vehicleBrand;
        }

        // PUT: api/VehicleBrands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleBrand(int id, VehicleBrand vehicleBrand)
        {
            if (id != vehicleBrand.VehicleBrandId)
            {
                return BadRequest();
            }

            _context.Entry(vehicleBrand).State = EntityState.Modified;

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VehicleBrand>> PostVehicleBrand(VehicleBrand vehicleBrand)
        {
            _context.VehicleBrands.Add(vehicleBrand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleBrand", new { id = vehicleBrand.VehicleBrandId }, vehicleBrand);
        }

        // DELETE: api/VehicleBrands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleBrand(int id)
        {
            var vehicleBrand = await _context.VehicleBrands.FindAsync(id);
            if (vehicleBrand == null)
            {
                return NotFound();
            }

            _context.VehicleBrands.Remove(vehicleBrand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleBrandExists(int id)
        {
            return _context.VehicleBrands.Any(e => e.VehicleBrandId == id);
        }
    }
}

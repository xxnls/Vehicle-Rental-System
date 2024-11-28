using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models.Other.Locations.DTOs;
using API.Models.Other.Locations;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public LocationsController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.LocationId)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateLocationDTO>> PostLocation(CreateLocationDTO createLocationDTO)
        {
            // Map CreateLocationDTO to Location entity
            var location = new Location
            {
                Gpslatitude = createLocationDTO.Gpslatitude,
                Gpslongitude = createLocationDTO.Gpslongitude,
                DateTime = createLocationDTO.DateTime,
                VehicleId = createLocationDTO.VehicleId,
                RentalPlaceId = createLocationDTO.RentalPlaceId
            };

            // Add the location to the context and save changes
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            // Map back the created location entity to a CreateLocationDTO for the response
            var locationDTO = new CreateLocationDTO
            {
                Gpslatitude = location.Gpslatitude,
                Gpslongitude = location.Gpslongitude,
                DateTime = location.DateTime,
                VehicleId = location.VehicleId,
                RentalPlaceId = location.RentalPlaceId
            };

            // Return the created location as a response
            return CreatedAtAction("GetLocation", new { id = location.LocationId }, locationDTO);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}

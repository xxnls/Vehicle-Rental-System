using API.Context;
using API.Interfaces;
using API.Models;
using API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class VehicleBrandsService : IBaseService<VehicleBrand, VehicleBrandDto, VehicleBrandDto>
    {
        private readonly ApiDbContext _context;

        public VehicleBrandsService(ApiDbContext context)
        {
            _context = context;
        }

        // Get all VehicleBrands with pagination and filtering
        public async Task<PaginatedResult<VehicleBrandDto>> GetAllAsync(
            string? search,
            int page,
            bool showDeleted,
            DateTime? createdBefore,
            DateTime? createdAfter,
            DateTime? modifiedBefore,
            DateTime? modifiedAfter,
            int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page and page size must be greater than 0.");
            }

            // Apply filter based on showDeleted
            IQueryable<VehicleBrand> query = !showDeleted ?
                _context.VehicleBrands.Where(vb => vb.IsActive) :
                _context.VehicleBrands.Where(vb => vb.IsActive == false);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(vb =>
                    vb.Name.Contains(search) ||
                    (vb.Description != null && vb.Description.Contains(search)) ||
                    (vb.Website != null && vb.Website.Contains(search)) ||
                    (vb.LogoUrl != null && vb.LogoUrl.Contains(search))
                );
            }

            // Apply date filters if provided
            if (createdBefore.HasValue)
            {
                query = query.Where(vb => vb.CreatedDate <= createdBefore.Value);
            }

            if (createdAfter.HasValue)
            {
                query = query.Where(vb => vb.CreatedDate >= createdAfter.Value);
            }

            if (modifiedBefore.HasValue)
            {
                query = query.Where(vb => vb.ModifiedDate <= modifiedBefore.Value);
            }

            if (modifiedAfter.HasValue)
            {
                query = query.Where(vb => vb.ModifiedDate >= modifiedAfter.Value);
            }

            // Get total count for pagination metadata
            int totalItemCount = await query.CountAsync();

            // Apply pagination
            var vehicleBrandDTOs = await query
                .OrderBy(vb => vb.VehicleBrandId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vb => new VehicleBrandDto
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
            return new PaginatedResult<VehicleBrandDto>
            {
                Items = vehicleBrandDTOs,
                TotalItemCount = totalItemCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        // Get VehicleBrand by Id
        public async Task<VehicleBrandDto> GetByIdAsync(int id)
        {
            var vehicleBrand = await _context.VehicleBrands
                .FirstOrDefaultAsync(vb => vb.VehicleBrandId == id);

            if (vehicleBrand == null)
                return null;

            return new VehicleBrandDto
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
        }

        // Create VehicleBrand
        public async Task<VehicleBrandDto> CreateAsync(VehicleBrandDto vehicleBrandDto)
        {
            var vehicleBrand = new VehicleBrand
            {
                Name = vehicleBrandDto.Name,
                Description = vehicleBrandDto.Description,
                Website = vehicleBrandDto.Website,
                LogoUrl = vehicleBrandDto.LogoUrl,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.VehicleBrands.Add(vehicleBrand);
            await _context.SaveChangesAsync();

            return new VehicleBrandDto
            {
                VehicleBrandId = vehicleBrand.VehicleBrandId,
                Name = vehicleBrand.Name,
                Description = vehicleBrand.Description,
                Website = vehicleBrand.Website,
                LogoUrl = vehicleBrand.LogoUrl,
                CreatedDate = vehicleBrand.CreatedDate,
                IsActive = vehicleBrand.IsActive
            };
        }

        // Update VehicleBrand
        public async Task<VehicleBrandDto> UpdateAsync(int id, VehicleBrandDto vehicleBrandDto)
        {
            var existingVehicleBrand = await _context.VehicleBrands
                .FirstOrDefaultAsync(vb => vb.VehicleBrandId == id);

            if (existingVehicleBrand == null)
                return null;

            existingVehicleBrand.Name = vehicleBrandDto.Name;
            existingVehicleBrand.Description = vehicleBrandDto.Description;
            existingVehicleBrand.Website = vehicleBrandDto.Website;
            existingVehicleBrand.LogoUrl = vehicleBrandDto.LogoUrl;
            existingVehicleBrand.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new VehicleBrandDto
            {
                VehicleBrandId = existingVehicleBrand.VehicleBrandId,
                Name = existingVehicleBrand.Name,
                Description = existingVehicleBrand.Description,
                Website = existingVehicleBrand.Website,
                LogoUrl = existingVehicleBrand.LogoUrl,
                CreatedDate = existingVehicleBrand.CreatedDate,
                ModifiedDate = existingVehicleBrand.ModifiedDate,
                DeletedDate = existingVehicleBrand.DeletedDate,
                IsActive = existingVehicleBrand.IsActive
            };
        }

        // Delete VehicleBrand (Soft Delete)
        public async Task<bool> DeleteAsync(int id)
        {
            var vehicleBrand = await _context.VehicleBrands
                .FirstOrDefaultAsync(vb => vb.VehicleBrandId == id);

            if (vehicleBrand == null)
                return false;

            vehicleBrand.DeletedDate = DateTime.UtcNow;
            vehicleBrand.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
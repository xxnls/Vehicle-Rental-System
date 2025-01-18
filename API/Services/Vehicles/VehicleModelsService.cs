using API.Context;
using API.Interfaces;
using API.Models;
using API.Models.DTOs.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Vehicles
{
    public class VehicleModelsService : IBaseService<VehicleModel, VehicleModelDto, VehicleModelDto>
    {
        private readonly ApiDbContext _context;

        public VehicleModelsService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<VehicleModelDto>> GetAllAsync(
            string? search,
            int page,
            bool showDeleted,
            DateTime? createdBefore,
            DateTime? createdAfter,
            DateTime? modifiedBefore,
            DateTime? modifiedAfter,
            int pageSize)
        {
            // Apply filter based on showDeleted
            IQueryable<VehicleModel> query = !showDeleted ?
                _context.VehicleModels.Where(vm => vm.IsActive) :
                _context.VehicleModels.Where(vm => vm.IsActive == false);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(vm =>
                    vm.VehicleModelId.ToString().Contains(search) ||
                    vm.EngineSize != null && vm.EngineSize.ToString().Contains(search) ||
                    vm.HorsePower != null && vm.HorsePower.ToString().Contains(search) ||
                    vm.VehicleBrandId.ToString().Contains(search) ||
                    vm.FuelType.Contains(search) ||
                    vm.Name.Contains(search) ||
                    vm.Description != null && vm.Description.Contains(search)
                );
            }

            // Apply date filters if provided
            if (createdBefore.HasValue)
            {
                query = query.Where(vm => vm.CreatedDate <= createdBefore.Value);
            }

            if (createdAfter.HasValue)
            {
                query = query.Where(vm => vm.CreatedDate >= createdAfter.Value);
            }

            if (modifiedBefore.HasValue)
            {
                query = query.Where(vm => vm.ModifiedDate <= modifiedBefore.Value);
            }

            if (modifiedAfter.HasValue)
            {
                query = query.Where(vm => vm.ModifiedDate >= modifiedAfter.Value);
            }

            // Get total count for pagination metadata
            int totalItemCount = await query.CountAsync();

            var vehicleModelDTOs = await query
                .OrderBy(vm => vm.VehicleModelId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(vm => new VehicleModelDto
                {
                    VehicleModelId = vm.VehicleModelId,
                    VehicleBrandId = vm.VehicleBrandId,
                    VehicleBrandName = vm.VehicleBrand.Name,
                    Name = vm.Name,
                    Description = vm.Description,
                    EngineSize = vm.EngineSize,
                    HorsePower = vm.HorsePower,
                    FuelType = vm.FuelType,
                    CreatedDate = vm.CreatedDate,
                    ModifiedDate = vm.ModifiedDate,
                    DeletedDate = vm.DeletedDate,
                    IsActive = vm.IsActive
                })
                .ToListAsync();

            // Return paginated result
            return new PaginatedResult<VehicleModelDto>
            {
                Items = vehicleModelDTOs,
                TotalItemCount = totalItemCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            var vehicleModel = await _context.VehicleModels
                .Include(vb => vb.VehicleBrand)
                .FirstOrDefaultAsync(vb => vb.VehicleModelId == id);

            if (vehicleModel == null)
            {
                throw new KeyNotFoundException("Vehicle model not found");
            }

            return new VehicleModelDto
            {
                VehicleModelId = vehicleModel.VehicleModelId,
                VehicleBrandId = vehicleModel.VehicleBrandId,
                VehicleBrandName = vehicleModel.VehicleBrand.Name,
                Name = vehicleModel.Name,
                Description = vehicleModel.Description,
                EngineSize = vehicleModel.EngineSize,
                HorsePower = vehicleModel.HorsePower,
                FuelType = vehicleModel.FuelType,
                CreatedDate = vehicleModel.CreatedDate,
                ModifiedDate = vehicleModel.ModifiedDate,
                DeletedDate = vehicleModel.DeletedDate,
                IsActive = vehicleModel.IsActive
            };
        }

        public async Task<VehicleModelDto> CreateAsync(VehicleModelDto vehicleModelDto)
        {
            var vehicleModel = new VehicleModel
            {
                VehicleBrandId = vehicleModelDto.VehicleBrandId,
                Name = vehicleModelDto.Name,
                Description = vehicleModelDto.Description,
                EngineSize = vehicleModelDto.EngineSize,
                HorsePower = vehicleModelDto.HorsePower,
                FuelType = vehicleModelDto.FuelType,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            return new VehicleModelDto
            {
                VehicleModelId = vehicleModel.VehicleModelId,
                VehicleBrandId = vehicleModel.VehicleBrandId,
                Name = vehicleModel.Name,
                Description = vehicleModel.Description,
                EngineSize = vehicleModel.EngineSize,
                HorsePower = vehicleModel.HorsePower,
                FuelType = vehicleModel.FuelType,
                CreatedDate = vehicleModel.CreatedDate,
                IsActive = vehicleModel.IsActive
            };
        }

        public async Task<VehicleModelDto> UpdateAsync(int id, VehicleModelDto entity)
        {
            var existingEntity = await _context.VehicleModels
                .FirstOrDefaultAsync(vb => vb.VehicleModelId == id);

            if (entity == null)
            {
                throw new KeyNotFoundException("Vehicle model not found");
            }

            existingEntity.VehicleBrandId = entity.VehicleBrandId;
            existingEntity.Name = entity.Name;
            existingEntity.Description = entity.Description;
            existingEntity.EngineSize = entity.EngineSize;
            existingEntity.HorsePower = entity.HorsePower;
            existingEntity.FuelType = entity.FuelType;
            existingEntity.ModifiedDate = DateTime.UtcNow;

            // Restoring deleted model
            if (entity.IsActive)
            {
                existingEntity.DeletedDate = entity.DeletedDate;
                existingEntity.IsActive = entity.IsActive;
            }

            await _context.SaveChangesAsync();

            return new VehicleModelDto
            {
                VehicleModelId = existingEntity.VehicleModelId,
                VehicleBrandId = existingEntity.VehicleBrandId,
                Name = existingEntity.Name,
                Description = existingEntity.Description,
                EngineSize = existingEntity.EngineSize,
                HorsePower = existingEntity.HorsePower,
                FuelType = existingEntity.FuelType,
                CreatedDate = existingEntity.CreatedDate,
                ModifiedDate = existingEntity.ModifiedDate,
                IsActive = existingEntity.IsActive
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingEntity = await _context.VehicleModels
                .FirstOrDefaultAsync(vb => vb.VehicleModelId == id);

            if (existingEntity == null)
                return false;

            existingEntity.DeletedDate = DateTime.UtcNow;
            existingEntity.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

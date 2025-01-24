using API.Context;
using API.Interfaces;
using API.Models;
using API.Models.DTOs.Vehicles;
using API.Services.Other;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.Vehicles
{
    public class VehicleModelsService : BaseApiService<VehicleModel, VehicleModelDto, VehicleModelDto>
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly VehicleBrandsService _vehicleBrandsService;

        public VehicleModelsService(ApiDbContext context, VehicleBrandsService vehicleBrandsService) : base(context)
        {
            _apiDbContext = context;
            _vehicleBrandsService = vehicleBrandsService;
        }

        protected override Expression<Func<VehicleModel, bool>> BuildSearchQuery(string search)
        {
            return vm =>
                vm.VehicleModelId.ToString().Contains(search) ||
                (vm.EngineSize != null && vm.EngineSize.ToString().Contains(search)) ||
                (vm.HorsePower != null && vm.HorsePower.ToString().Contains(search)) ||
                vm.VehicleBrandId.ToString().Contains(search) ||
                vm.VehicleBrand.Name.Contains(search) ||
                vm.FuelType.Contains(search) ||
                vm.Name.Contains(search) ||
                (vm.Description != null && vm.Description.Contains(search));
        }

        protected override Expression<Func<VehicleModel, bool>> GetActiveFilter(bool showDeleted)
        {
            return vm => vm.IsActive != showDeleted;
        }

        public override VehicleModel MapToEntity(VehicleModelDto model)
        {
            return new VehicleModel
            {
                VehicleModelId = model.VehicleModelId,
                Name = model.Name,
                Description = model.Description,
                EngineSize = model.EngineSize,
                HorsePower = model.HorsePower,
                FuelType = model.FuelType,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive,
                
                VehicleBrandId = model.VehicleBrand.VehicleBrandId
            };
        }

        public override Expression<Func<VehicleModel, VehicleModelDto>> MapToDto()
        {
            return vm => new VehicleModelDto
            {
                VehicleModelId = vm.VehicleModelId,
                Name = vm.Name,
                Description = vm.Description,
                EngineSize = vm.EngineSize,
                HorsePower = vm.HorsePower,
                FuelType = vm.FuelType,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                DeletedDate = vm.DeletedDate,
                IsActive = vm.IsActive,

                VehicleBrand = vm.VehicleBrand != null ? _vehicleBrandsService.MapSingleEntityToDto(vm.VehicleBrand) : null
            };
        }

        public override VehicleModelDto MapSingleEntityToDto(VehicleModel entity)
        {
            return new VehicleModelDto
            {
                VehicleModelId = entity.VehicleModelId,
                Name = entity.Name,
                Description = entity.Description,
                EngineSize = entity.EngineSize,
                HorsePower = entity.HorsePower,
                FuelType = entity.FuelType,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,

                VehicleBrand = entity.VehicleBrand != null ? _vehicleBrandsService.MapSingleEntityToDto(entity.VehicleBrand) : null
            };
        }

        public override async Task<VehicleModelDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleModel).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleModel entity, VehicleModelDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.EngineSize = model.EngineSize;
            entity.HorsePower = model.HorsePower;
            entity.FuelType = model.FuelType;
            entity.VehicleBrandId = model.VehicleBrand.VehicleBrandId;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<VehicleModel> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleModels
                .Include(vm => vm.VehicleBrand)
                .FirstOrDefaultAsync(vm => vm.VehicleModelId == id);
        }

        protected override IQueryable<VehicleModel> IncludeRelatedEntities(IQueryable<VehicleModel> query)
        {
            return query
                .Include(v => v.VehicleBrand);
        }
    }
}

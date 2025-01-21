using API.Context;
using API.Models.DTOs.Vehicles;
using System.Linq.Expressions;
using API.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Vehicles
{
    public class VehicleTypesService : BaseApiService<VehicleType, VehicleTypeDto, VehicleTypeDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public VehicleTypesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<VehicleType, bool>> BuildSearchQuery(string search)
        {
            return vt =>
                vt.VehicleTypeId.ToString().Contains(search) ||
                vt.Name.Contains(search) ||
                (vt.Description != null && vt.Description.Contains(search)) ||
                vt.BaseDailyRate.ToString().Contains(search) ||
                vt.BaseWeeklyRate.ToString().Contains(search) ||
                vt.BaseDeposit.ToString().Contains(search) ||
                vt.RequiredLicenseType.Contains(search);
        }

        protected override Expression<Func<VehicleType, bool>> GetActiveFilter(bool showDeleted)
        {
            return vt => vt.IsActive != showDeleted;
        }

        public override VehicleType MapToEntity(VehicleTypeDto model)
        {
            return new VehicleType
            {
                VehicleTypeId = model.VehicleTypeId,
                Name = model.Name,
                Description = model.Description,
                BaseDailyRate = model.BaseDailyRate,
                BaseWeeklyRate = model.BaseWeeklyRate,
                BaseDeposit = model.BaseDeposit,
                RequiredLicenseType = model.RequiredLicenseType,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<VehicleType, VehicleTypeDto>> MapToDto()
        {
            return vt => new VehicleTypeDto
            {
                VehicleTypeId = vt.VehicleTypeId,
                Name = vt.Name,
                Description = vt.Description,
                BaseDailyRate = vt.BaseDailyRate,
                BaseWeeklyRate = vt.BaseWeeklyRate,
                BaseDeposit = vt.BaseDeposit,
                RequiredLicenseType = vt.RequiredLicenseType,
                CreatedDate = vt.CreatedDate,
                ModifiedDate = vt.ModifiedDate,
                DeletedDate = vt.DeletedDate,
                IsActive = vt.IsActive
            };
        }

        public override VehicleTypeDto MapSingleEntityToDto(VehicleType entity)
        {
            return new VehicleTypeDto
            {
                VehicleTypeId = entity.VehicleTypeId,
                Name = entity.Name,
                Description = entity.Description,
                BaseDailyRate = entity.BaseDailyRate,
                BaseWeeklyRate = entity.BaseWeeklyRate,
                BaseDeposit = entity.BaseDeposit,
                RequiredLicenseType = entity.RequiredLicenseType,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        public override async Task<VehicleTypeDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleType).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleType entity, VehicleTypeDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.BaseDailyRate = model.BaseDailyRate;
            entity.BaseWeeklyRate = model.BaseWeeklyRate;
            entity.BaseDeposit = model.BaseDeposit;
            entity.RequiredLicenseType = model.RequiredLicenseType;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<VehicleType> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.VehicleTypeId == id);
        }
    }
}


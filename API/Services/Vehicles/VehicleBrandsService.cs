using API.Context;
using API.Interfaces;
using API.Models;
using API.Models.DTOs.Vehicles;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services
{
    public class VehicleBrandsService : BaseApiService<VehicleBrand, VehicleBrandDto, VehicleBrandDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public VehicleBrandsService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<VehicleBrand, bool>> BuildSearchQuery(string search)
        {
            return vb =>
                vb.VehicleBrandId.ToString().Contains(search) ||
                vb.Name.Contains(search) ||
                (vb.Description != null && vb.Description.Contains(search)) ||
                (vb.Website != null && vb.Website.Contains(search)) ||
                (vb.LogoUrl != null && vb.LogoUrl.Contains(search));
        }

        protected override Expression<Func<VehicleBrand, bool>> GetActiveFilter(bool showDeleted)
        {
            return vb => vb.IsActive != showDeleted;
        }

        public override VehicleBrand MapToEntity(VehicleBrandDto model)
        {
            return new VehicleBrand
            {
                VehicleBrandId = model.VehicleBrandId,
                Name = model.Name,
                Description = model.Description,
                Website = model.Website,
                LogoUrl = model.LogoUrl,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<VehicleBrand, VehicleBrandDto>> MapToDto()
        {
            return vb => new VehicleBrandDto
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
            };
        }

        public override VehicleBrandDto MapSingleEntityToDto(VehicleBrand entity)
        {
            return new VehicleBrandDto
            {
                VehicleBrandId = entity.VehicleBrandId,
                Name = entity.Name,
                Description = entity.Description,
                Website = entity.Website,
                LogoUrl = entity.LogoUrl,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        public override async Task<VehicleBrandDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleBrand).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleBrand entity, VehicleBrandDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Website = model.Website;
            entity.LogoUrl = model.LogoUrl;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<VehicleBrand> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleBrands
                .FirstOrDefaultAsync(vb => vb.VehicleBrandId == id);
        }
    }
}
using API.Context;
using API.Models.DTOs.Employees;
using System.Linq.Expressions;
using API.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeShiftTypesService : BaseApiService<EmployeeShiftType, EmployeeShiftTypeDto, EmployeeShiftTypeDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public EmployeeShiftTypesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<EmployeeShiftType, bool>> BuildSearchQuery(string search)
        {
            return est =>
                est.EmployeeShiftTypeId.ToString().Contains(search) ||
                (est.Name != null && est.Name.Contains(search)) ||
                est.TimeStart.ToString().Contains(search) ||
                est.TimeEnd.ToString().Contains(search);
        }

        protected override Expression<Func<EmployeeShiftType, bool>> GetActiveFilter(bool showDeleted)
        {
            return est => est.IsActive != showDeleted;
        }

        public override EmployeeShiftType MapToEntity(EmployeeShiftTypeDto model)
        {
            return new EmployeeShiftType
            {
                EmployeeShiftTypeId = model.EmployeeShiftTypeId,
                Name = model.Name,
                TimeStart = model.TimeStart,
                TimeEnd = model.TimeEnd,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<EmployeeShiftType, EmployeeShiftTypeDto>> MapToDto()
        {
            return est => new EmployeeShiftTypeDto
            {
                EmployeeShiftTypeId = est.EmployeeShiftTypeId,
                Name = est.Name,
                TimeStart = est.TimeStart,
                TimeEnd = est.TimeEnd,
                CreatedDate = est.CreatedDate,
                ModifiedDate = est.ModifiedDate,
                DeletedDate = est.DeletedDate,
                IsActive = est.IsActive
            };
        }

        public override EmployeeShiftTypeDto MapSingleEntityToDto(EmployeeShiftType entity)
        {
            return new EmployeeShiftTypeDto
            {
                EmployeeShiftTypeId = entity.EmployeeShiftTypeId,
                Name = entity.Name,
                TimeStart = entity.TimeStart,
                TimeEnd = entity.TimeEnd,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        public override async Task<EmployeeShiftTypeDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(EmployeeShiftType).Name} not found");
            }
            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(EmployeeShiftType entity, EmployeeShiftTypeDto model)
        {
            entity.Name = model.Name;
            entity.TimeStart = model.TimeStart;
            entity.TimeEnd = model.TimeEnd;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<EmployeeShiftType> FindEntityById(int id)
        {
            return await _apiDbContext.EmployeeShiftTypes
                .FirstOrDefaultAsync(est => est.EmployeeShiftTypeId == id);
        }

        protected override IQueryable<EmployeeShiftType> IncludeRelatedEntities(IQueryable<EmployeeShiftType> query)
        {
            return query; // No related entities to include
        }
    }
}

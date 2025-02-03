using API.Context;
using API.Models.DTOs.Employees;
using System.Linq.Expressions;
using API.Models;
using API.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeePositionsService : BaseApiService<EmployeePosition, EmployeePositionDto, EmployeePositionDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public EmployeePositionsService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<EmployeePosition, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.EmployeePositionId.ToString().Contains(search) ||
                e.Title.Contains(search) ||
                (e.DefaultBaseSalary != null && e.DefaultBaseSalary.ToString().Contains(search)) ||
                (e.DefaultHourlyRate != null && e.DefaultHourlyRate.ToString().Contains(search)) ||
                e.CreatedDate.ToString().Contains(search) ||
                (e.ModifiedDate != null && e.ModifiedDate.ToString().Contains(search)) ||
                (e.DeletedDate != null && e.DeletedDate.ToString().Contains(search)) ||
                e.IsActive.ToString().Contains(search);
        }

        public override EmployeePosition MapToEntity(EmployeePositionDto model)
        {
            return new EmployeePosition
            {
                EmployeePositionId = model.EmployeePositionId,
                Title = model.Title,
                DefaultBaseSalary = model.DefaultBaseSalary,
                DefaultHourlyRate = model.DefaultHourlyRate,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<EmployeePosition, EmployeePositionDto>> MapToDto()
        {
            return e => new EmployeePositionDto
            {
                EmployeePositionId = e.EmployeePositionId,
                Title = e.Title,
                DefaultBaseSalary = e.DefaultBaseSalary,
                DefaultHourlyRate = e.DefaultHourlyRate,
                CreatedDate = e.CreatedDate,
                ModifiedDate = e.ModifiedDate,
                DeletedDate = e.DeletedDate,
                IsActive = e.IsActive
            };
        }

        public override EmployeePositionDto MapSingleEntityToDto(EmployeePosition entity)
        {
            return new EmployeePositionDto
            {
                EmployeePositionId = entity.EmployeePositionId,
                Title = entity.Title,
                DefaultBaseSalary = entity.DefaultBaseSalary,
                DefaultHourlyRate = entity.DefaultHourlyRate,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        protected override Expression<Func<EmployeePosition, bool>> GetActiveFilter(bool showDeleted)
        {
            return est => est.IsActive != showDeleted;
        }

        public override async Task<EmployeePositionDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(EmployeePosition).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(EmployeePosition entity, EmployeePositionDto model)
        {
            entity.Title = model.Title;
            entity.DefaultBaseSalary = model.DefaultBaseSalary;
            entity.DefaultHourlyRate = model.DefaultHourlyRate;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<EmployeePosition> FindEntityById(int id)
        {
            return await _apiDbContext.EmployeePositions
                .FirstOrDefaultAsync(e => e.EmployeePositionId == id);
        }
    }
}
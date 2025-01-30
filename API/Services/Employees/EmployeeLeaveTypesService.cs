using API.Context;
using API.Models.DTOs.Employees;
using System.Linq.Expressions;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeLeaveTypesService : BaseApiService<EmployeeLeaveType, EmployeeLeaveTypeDto, EmployeeLeaveTypeDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public EmployeeLeaveTypesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<EmployeeLeaveType, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.EmployeeLeaveTypeId.ToString().Contains(search) ||
                e.Name.Contains(search) ||
                (e.Description != null && e.Description.Contains(search)) ||
                e.DefaultDays.ToString().Contains(search);
        }


        public override EmployeeLeaveType MapToEntity(EmployeeLeaveTypeDto model)
        {
            return new EmployeeLeaveType
            {
                EmployeeLeaveTypeId = model.EmployeeLeaveTypeId,
                Name = model.Name,
                Description = model.Description,
                DefaultDays = model.DefaultDays,
            };
        }

        public override Expression<Func<EmployeeLeaveType, EmployeeLeaveTypeDto>> MapToDto()
        {
            return e => new EmployeeLeaveTypeDto
            {
                EmployeeLeaveTypeId = e.EmployeeLeaveTypeId,
                Name = e.Name,
                Description = e.Description,
                DefaultDays = e.DefaultDays
            };
        }

        public override EmployeeLeaveTypeDto MapSingleEntityToDto(EmployeeLeaveType entity)
        {
            return new EmployeeLeaveTypeDto
            {
                EmployeeLeaveTypeId = entity.EmployeeLeaveTypeId,
                Name = entity.Name,
                Description = entity.Description,
                DefaultDays = entity.DefaultDays
            };
        }

        public override async Task<EmployeeLeaveTypeDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(EmployeeLeaveType).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(EmployeeLeaveType entity, EmployeeLeaveTypeDto model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DefaultDays = model.DefaultDays;
        }

        public override async Task<EmployeeLeaveType> FindEntityById(int id)
        {
            return await _apiDbContext.EmployeeLeaveTypes
                .FirstOrDefaultAsync(e => e.EmployeeLeaveTypeId == id);
        }
    }
}

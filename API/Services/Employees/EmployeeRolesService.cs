using System.Linq.Expressions;
using API.Context;
using API.Models;
using API.Models.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeRolesService : BaseApiService<EmployeeRole, EmployeeRoleDto, EmployeeRoleDto>
    {
        private readonly ApiDbContext _context;

        public EmployeeRolesService(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        // Build a search query for roles
        protected override Expression<Func<EmployeeRole, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.Id.ToString().Contains(search) || // Role ID
                e.Name.Contains(search) || // Role name
                e.RolePower.ToString().Contains(search) || // Role power
                e.ManageVehicles.ToString().Contains(search) || // Manage vehicles permission
                e.ManageEmployees.ToString().Contains(search) || // Manage employees permission
                e.ManageRentals.ToString().Contains(search) || // Manage rentals permission
                e.ManageLeaves.ToString().Contains(search) || // Manage leaves permission
                e.ManageSchedule.ToString().Contains(search) || // Manage schedule permission
                (e.ModifiedDate != null && e.ModifiedDate.ToString().Contains(search)); // Modified date
        }

        #region Mapping

        // Map a DTO to an EmployeeRole entity
        public override EmployeeRole MapToEntity(EmployeeRoleDto dto)
        {
            return new EmployeeRole
            {
                Id = dto.Id,
                Name = dto.Name,
                RolePower = dto.RolePower,
                ManageVehicles = dto.ManageVehicles,
                ManageEmployees = dto.ManageEmployees,
                ManageRentals = dto.ManageRentals,
                ManageLeaves = dto.ManageLeaves,
                ManageSchedule = dto.ManageSchedule,
                ModifiedDate = DateTime.UtcNow
            };
        }

        // Map an EmployeeRole entity to a DTO
        public override Expression<Func<EmployeeRole, EmployeeRoleDto>> MapToDto()
        {
            return e => new EmployeeRoleDto
            {
                Id = e.Id,
                Name = e.Name!,
                RolePower = e.RolePower,
                ManageVehicles = e.ManageVehicles,
                ManageEmployees = e.ManageEmployees,
                ManageRentals = e.ManageRentals,
                ManageLeaves = e.ManageLeaves,
                ManageSchedule = e.ManageSchedule,
                ModifiedDate = e.ModifiedDate
            };
        }

        // Map a single EmployeeRole entity to a DTO
        public override EmployeeRoleDto MapSingleEntityToDto(EmployeeRole entity)
        {
            return new EmployeeRoleDto
            {
                Id = entity.Id,
                Name = entity.Name!,
                RolePower = entity.RolePower,
                ManageVehicles = entity.ManageVehicles,
                ManageEmployees = entity.ManageEmployees,
                ManageRentals = entity.ManageRentals,
                ManageLeaves = entity.ManageLeaves,
                ManageSchedule = entity.ManageSchedule,
                ModifiedDate = entity.ModifiedDate
            };
        }

        #endregion

        // Update an EmployeeRole entity with data from a DTO
        protected override void UpdateEntity(EmployeeRole entity, EmployeeRoleDto dto)
        {
            entity.Name = dto.Name;
            entity.RolePower = dto.RolePower;
            entity.ManageVehicles = dto.ManageVehicles;
            entity.ManageEmployees = dto.ManageEmployees;
            entity.ManageRentals = dto.ManageRentals;
            entity.ManageLeaves = dto.ManageLeaves;
            entity.ManageSchedule = dto.ManageSchedule;
            entity.ModifiedDate = DateTime.UtcNow;
        }

        // Find an EmployeeRole entity by its ID
        public override async Task<EmployeeRole> FindEntityById(int id)
        {
            return await _context.EmployeeRoles
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}

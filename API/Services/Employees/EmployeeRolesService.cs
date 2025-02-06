using System.Linq.Expressions;
using API.Context;
using API.Models;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeRolesService : BaseApiService<EmployeeRole, EmployeeRoleDto, EmployeeRoleDto>
    {
        private readonly RoleManager<EmployeeRole> _roleManager;
        private readonly UserManager<Employee> _userManager;
        private readonly ApiDbContext _context;

        public EmployeeRolesService(
            ApiDbContext context,
            RoleManager<EmployeeRole> roleManager,
            UserManager<Employee> userManager) : base(context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        #region Role Management

        /// <summary>
        /// Get the roles of a user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user to get the roles of.
        /// </param>
        /// <returns>Roles in a list.</returns>
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return []; // Return empty if user not found

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        /// <summary>
        /// Assign user to a specified role.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user to assign the role to.
        /// </param>
        /// <param name="roleName">
        /// The name of the role to assign.
        /// </param>
        /// <returns>
        /// True if the role was assigned successfully; otherwise, false.
        /// </returns>
        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false; // Return false if user not found
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        /// <summary>
        /// Remove a role from a user.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user to remove the role from.
        /// </param>
        /// <param name="roleName">
        /// The name of the role to remove.
        /// </param>
        /// <returns>
        /// True if the role was removed successfully; otherwise, false.
        /// </returns>
        public async Task<bool> RemoveRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false; // Return false if user not found
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        /// <summary>
        /// Check if a user has a specific permission.
        /// </summary>
        /// <param name="userId">
        /// The ID of the user to check the permission for.
        /// </param>
        /// <param name="permission">
        /// The permission to check (e.g., ManageVehicles).
        /// </param>
        /// <returns>
        /// True if the user has the permission; otherwise, false.
        /// </returns>
        public async Task<bool> HasPermissionAsync(string userId, string permission)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false; // Return false if user not found

            var roles = await _userManager.GetRolesAsync(user);

            // Fetch the user's role with permissions from the database
            var role = await _context.EmployeeRoles
                .Where(r => roles.Contains(r.Name))
                .Select(r => new
                {
                    r.ManageVehicles,
                    r.ManageEmployees,
                    r.ManageRentals,
                    r.ManageLeaves,
                    r.ManageSchedule
                })
                .FirstOrDefaultAsync();

            if (role == null)
                return false;

            // Map permission name to the corresponding property
            return permission switch
            {
                "ManageVehicles" => role.ManageVehicles,
                "ManageEmployees" => role.ManageEmployees,
                "ManageRentals" => role.ManageRentals,
                "ManageLeaves" => role.ManageLeaves,
                "ManageSchedule" => role.ManageSchedule,
                _ => false
            };
        }

        #endregion

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
            return await _roleManager.Roles.FirstOrDefaultAsync(e => e.Id == id);
        }

        // Create a new role
        public override async Task<EmployeeRoleDto> CreateAsync(EmployeeRoleDto dto)
        {
            var role = MapToEntity(dto);

            // Check if the role already exists
            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                throw new InvalidOperationException($"Role '{role.Name}' already exists.");
            }

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Role creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return MapSingleEntityToDto(role);
        }

        // Update an existing role
        public override async Task<EmployeeRoleDto> UpdateAsync(int id, EmployeeRoleDto dto)
        {
            var role = await FindEntityById(id);

            if (role == null)
            {
                throw new KeyNotFoundException("Role not found.");
            }

            UpdateEntity(role, dto);

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Role update failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return MapSingleEntityToDto(role);
        }

        // Delete a role
        public override async Task<bool> DeleteAsync(int id)
        {
            var role = await FindEntityById(id);

            if (role == null)
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Role deletion failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return true;
        }
    }
}
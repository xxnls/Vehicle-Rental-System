using API.Models.DTOs.Employees;
using API.Services.Employees;

namespace API.Seeders
{
    public class EmployeeRolesSeeder
    {
        private readonly EmployeeRolesService _rolesService;

        public EmployeeRolesSeeder(EmployeeRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public static async Task SeedAsync(EmployeeRolesService rolesService)
        {
            var existingRoles = await rolesService.GetAllAsync();
            if (existingRoles != null && existingRoles.TotalItemCount > 0)
            {
                return; // Data is already seeded
            }

            var roles = new List<EmployeeRoleDto>
            {
                new() { Name = "Admin", RolePower = 999, ManageVehicles = true, ManageEmployees = true, ManageRentals = true, ManageLeaves = true, ManageSchedule = true },
                new() { Name = "Manager", RolePower = 700, ManageVehicles = true, ManageEmployees = true, ManageRentals = true, ManageLeaves = true, ManageSchedule = true },
                new() { Name = "Fleet Manager", RolePower = 500, ManageVehicles = true, ManageEmployees = false, ManageRentals = true, ManageLeaves = false, ManageSchedule = false },
                new() { Name = "HR Manager", RolePower = 500, ManageVehicles = false, ManageEmployees = true, ManageRentals = false, ManageLeaves = true, ManageSchedule = true },
                new() { Name = "Customer Service", RolePower = 300, ManageVehicles = false, ManageEmployees = false, ManageRentals = true, ManageLeaves = false, ManageSchedule = false },
                new() { Name = "Fleet Operator", RolePower = 300, ManageVehicles = true, ManageEmployees = false, ManageRentals = false, ManageLeaves = false, ManageSchedule = false },
                new() { Name = "Intern", RolePower = 100, ManageVehicles = false, ManageEmployees = false, ManageRentals = false, ManageLeaves = false, ManageSchedule = false },
                new() { Name = "Default", RolePower = 0, ManageVehicles = false, ManageEmployees = false, ManageRentals = false, ManageLeaves = false, ManageSchedule = false }
            };

            // Add roles to the database
            foreach (var role in roles)
            {
                await rolesService.CreateAsync(role);
            }
        }
    }
}

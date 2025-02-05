using API.Models.DTOs.Employees;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Seeders
{
    public class EmployeeRolesSeeder
    {
        private readonly RoleManager<EmployeeRole> _roleManager;

        public EmployeeRolesSeeder(RoleManager<EmployeeRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public static async Task SeedAsync(RoleManager<EmployeeRole> roleManager)
        {
            var roles = new List<EmployeeRole>
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
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
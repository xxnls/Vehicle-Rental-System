using API.Models;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Other;
using API.Models.Employees;
using API.Services.Employees;
using API.Services.Other;
using Microsoft.AspNetCore.Identity;

namespace API.Seeders
{
    public class EmployeeSeeder
    {
        private readonly EmployeesService _employeesService;
        private readonly EmployeeRolesService _roleManager;
        private readonly EmployeePositionsService _employeePositionsService;
        private readonly RentalPlacesService _rentalPlacesService;
        private readonly UserManager<Employee> _userManager;

        public EmployeeSeeder(EmployeesService employeesService, EmployeeRolesService rolesManager, EmployeePositionsService employeePositionsService, RentalPlacesService rentalPlacesService, UserManager<Employee> userManager)
        {
            _employeesService = employeesService;
            _roleManager = rolesManager;
            _employeePositionsService = employeePositionsService;
            _rentalPlacesService = rentalPlacesService;
            _userManager = userManager;
        }

        public static async Task SeedAsync(EmployeesService employeesService, EmployeeRolesService rolesManager,
            EmployeePositionsService employeePositionsService, RentalPlacesService rentalPlacesService, UserManager<Employee> userManager)
        {
            try
            {
                // Create new employee position
                var existingPositions = await employeePositionsService.GetAllAsync();
                if (existingPositions.TotalItemCount == 0)
                {
                    var position = new EmployeePositionDto { Title = "Admin" };
                    await employeePositionsService.CreateAsync(position);
                }

                // Create new rental place
                var existingRentalPlaces = await rentalPlacesService.GetAllAsync();
                if (existingRentalPlaces.TotalItemCount == 0)
                {
                    var rentalPlace = new RentalPlaceDto
                    {
                        Location = new LocationDto { Gpslatitude = 00.01, Gpslongitude = 00.01 },
                        Address = new AddressDto
                        {
                            FirstLine = "Rental Place First Line",
                            City = "Rental Place City",
                            Country = new CountryDto
                            {
                                CountryId = 1,
                                FullName = "Republic of Poland",
                                Abbreviation = "PL",
                                DialingCode = "+48"
                            },
                            ZipCode = "12345"
                        }
                    };

                    await rentalPlacesService.CreateAsync(rentalPlace);
                }

                // Create Admin employee
                var existingEmployees = await employeesService.GetAllAsync();
                if (existingEmployees.TotalItemCount == 0)
                {
                    var admin = new EmployeeDto
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Email = "admin@admin.com",
                        PhoneNumber = "123456789",
                        Password = "123",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        HireDate = DateTime.Now,
                        EmployeePosition = new EmployeePositionDto { EmployeePositionId = 1 },
                        RentalPlace = new RentalPlaceDto { RentalPlaceId = 1 },
                        Address = new AddressDto
                        {
                            FirstLine = "Admin Address",
                            City = "Admin City",
                            Country = new CountryDto
                            {
                                CountryId = 1,
                                FullName = "Republic of Poland",
                                Abbreviation = "PL",
                                DialingCode = "+48"
                            },
                            ZipCode = "12345"
                        },
                        EmployeeFinances = new EmployeeFinanceDto { BaseSalary = 0 }
                    };

                    var createdAdmin = await employeesService.CreateAsync(admin);
                    await rolesManager.RemoveRoleAsync(createdAdmin.Id.ToString(), "Default");
                    await rolesManager.AssignRoleAsync(createdAdmin.Id.ToString(), "Admin");

                    // Change username
                    //createdAdmin.UserName = "admin";
                    //await employeesService.UpdateAsync(createdAdmin.Id, createdAdmin);
                    await userManager.SetUserNameAsync(await employeesService.FindEntityById(createdAdmin.Id), "admin");
                }

                Console.WriteLine("Admin user created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating admin user: {ex.Message}");
            }
        }
    }
}

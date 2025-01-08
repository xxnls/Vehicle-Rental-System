using API.Context;
using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class EmployeesService
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<Employee> _userManager;

        public EmployeesService(ApiDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Create Employee
        public async Task<REmployeeDTO> CreateEmployeeAsync(CUEmployeeDTO employeeDto)
        {
            var employeeStatistics = new EmployeeStatistic
            {
                TotalWorkDays = 0,
                LateArrivals = 0,
                EarlyDepartures = 0,
                OvertimeHours = 0,
                SickLeavesTaken = 0,
                VacationDaysTaken = 0,
                UnpaidLeavesTaken = 0,
                TotalRentalsApproved = 0
            };

            var employeeFinances = new EmployeeFinance
            {
                BaseSalary = null,
                HourlyRate = null,
                ModifiedDate = null
            };

            // Save the related entities first
            _context.EmployeeStatistics.Add(employeeStatistics);
            _context.EmployeeFinances.Add(employeeFinances);
            await _context.SaveChangesAsync();

            // Generate UserName
            Random rnd = new Random();
            var generatedUserName = $"{employeeDto.FirstName.ToLower()}.{employeeDto.LastName.ToLower()}{rnd.Next(1000):000}";

            var employee = new Employee
            {
                UserName = generatedUserName,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,

                RentalPlaceId = employeeDto.RentalPlaceId,
                AddressId = employeeDto.AddressId,
                EmployeePositionId = employeeDto.EmployeePositionId,
                SupervisorId = employeeDto.SupervisorId,

                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth,
                HireDate = employeeDto.HireDate,
                TerminationDate = employeeDto.TerminationDate,
                Status = employeeDto.Status,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,

                // Assign related entities' IDs
                EmployeeStatisticsId = employeeStatistics.EmployeeStatisticsId,
                EmployeeFinancesId = employeeFinances.EmployeeFinancesId
            };

            // Create a new employee (which is also a user)
            var result = await _userManager.CreateAsync(employee, employeeDto.Password);

            if (result.Succeeded)
            {
                var employeeResult = new REmployeeDTO
                {
                    Id = employee.Id,
                    EmployeeStatisticsId = employee.EmployeeStatisticsId,
                    EmployeeFinancesId = employee.EmployeeFinancesId,
                    RentalPlaceId = employee.RentalPlaceId,
                    AddressId = employee.AddressId,
                    EmployeePositionId = employee.EmployeePositionId,
                    SupervisorId = employee.SupervisorId,
                    Status = employee.Status,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DateOfBirth = employee.DateOfBirth,
                    HireDate = employee.HireDate,
                    TerminationDate = employee.TerminationDate,
                    CreatedDate = employee.CreatedDate,
                    ModifiedDate = employee.ModifiedDate,
                    DeletedDate = employee.DeletedDate,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    UserName = employee.UserName
                };

                return employeeResult;
            }
            else
            {
                // Log the errors for debugging
                string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Employee creation failed: {errorMessages}");
            }
        }

        // Get Employee by Id
        public async Task<REmployeeDTO> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return null;

            var employeeDto = new REmployeeDTO
            {
                EmployeeStatisticsId = employee.EmployeeStatisticsId,
                EmployeeFinancesId = employee.EmployeeFinancesId,
                RentalPlaceId = employee.RentalPlaceId,
                AddressId = employee.AddressId,
                EmployeePositionId = employee.EmployeePositionId,
                SupervisorId = employee.SupervisorId,
                Status = employee.Status,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                TerminationDate = employee.TerminationDate,
                CreatedDate = employee.CreatedDate,
                ModifiedDate = employee.ModifiedDate,
                DeletedDate = employee.DeletedDate,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                UserName = employee.UserName
            };

            return employeeDto;
        }

        // Get all Employees
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        // Update Employee
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existingEmployee == null)
                return null;

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.TerminationDate = employee.TerminationDate;
            existingEmployee.Status = employee.Status;
            existingEmployee.ModifiedDate = DateTime.Now;

            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        // Delete Employee
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return false;

            // Set DeletedDate and IsActive for soft delete
            employee.DeletedDate = DateTime.Now;
            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

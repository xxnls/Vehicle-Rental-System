using API.Context;
using API.Models;
using API.Models.DTOs;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.Models.DTOs.Other;
using API.Services.Other;

namespace API.Services.Employees
{
    public class EmployeesService : BaseApiService<Employee, EmployeeDto, EmployeeDto>
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly EmployeeFinancesService _employeeFinancesService;
        private readonly EmployeeStatisticsService _employeeStatisticsService;
        private readonly AddressesService _addressesService;
        private readonly EmployeePositionsService _employeePositionsService;
        private readonly RentalPlacesService _rentalPlacesService;

        public EmployeesService(
            ApiDbContext context,
            UserManager<Employee> userManager,
            EmployeeFinancesService employeeFinancesService,
            EmployeeStatisticsService employeeStatisticsService,
            AddressesService addressesService, EmployeePositionsService employeePositionsService,
            RentalPlacesService rentalPlacesService) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _employeeFinancesService = employeeFinancesService;
            _employeeStatisticsService = employeeStatisticsService;
            _addressesService = addressesService;
            _employeePositionsService = employeePositionsService;
            _rentalPlacesService = rentalPlacesService;
        }

        protected override Expression<Func<Employee, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.Id.ToString().Contains(search) ||
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                e.Email.Contains(search) ||
                e.PhoneNumber.Contains(search) ||
                e.UserName.Contains(search) ||
                e.Supervisor.FirstName.Contains(search) ||
                e.Supervisor.LastName.Contains(search) ||
                e.DateOfBirth.ToString().Contains(search) ||
                (e.Status != null && e.Status.Contains(search)) ||
                e.HireDate.ToString().Contains(search) ||
                (e.TerminationDate != null && e.TerminationDate.ToString().Contains(search)) ||
                e.EmployeePosition.Title.Contains(search) ||
                (e.EmployeeFinances.BaseSalary.HasValue &&
                 e.EmployeeFinances.BaseSalary.Value.ToString().Contains(search)) ||
                (e.EmployeeFinances.HourlyRate.HasValue &&
                 e.EmployeeFinances.HourlyRate.Value.ToString().Contains(search)) ||
                e.RentalPlace.Address.City.Contains(search) ||
                e.RentalPlace.Address.FirstLine.Contains(search) ||
                e.RentalPlace.Address.SecondLine.Contains(search) ||
                e.RentalPlace.Address.ZipCode.Contains(search);
        }

        protected override Expression<Func<Employee, bool>> GetActiveFilter(bool showDeleted)
        {
            return v => v.IsActive != showDeleted;
        }

        #region Mapping

        public override Employee MapToEntity(EmployeeDto model)
        {
            return new Employee
            {
                Id = model.Id,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                RentalPlaceId = model.RentalPlaceId,
                AddressId = model.AddressId,
                EmployeePositionId = model.EmployeePositionId,
                SupervisorId = model.SupervisorId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                HireDate = model.HireDate,
                TerminationDate = model.TerminationDate,
                Status = model.Status,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive,
                EmployeeStatisticsId = model.EmployeeStatisticsId,
                EmployeeFinancesId = model.EmployeeFinancesId
            };
        }

        public override Expression<Func<Employee, EmployeeDto>> MapToDto()
        {
            return e => new EmployeeDto
            {
                Id = e.Id,
                UserName = e.UserName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                RentalPlaceId = e.RentalPlaceId,
                AddressId = e.AddressId,
                EmployeePositionId = e.EmployeePositionId,
                SupervisorId = e.SupervisorId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                DateOfBirth = e.DateOfBirth,
                HireDate = e.HireDate,
                TerminationDate = e.TerminationDate,
                Status = e.Status,
                CreatedDate = e.CreatedDate,
                ModifiedDate = e.ModifiedDate,
                DeletedDate = e.DeletedDate,
                IsActive = e.IsActive,
                EmployeeStatisticsId = e.EmployeeStatisticsId,
                EmployeeFinancesId = e.EmployeeFinancesId,
                EmployeeFinances = e.EmployeeFinances != null ? _employeeFinancesService.MapSingleEntityToDto(e.EmployeeFinances) : null,
                EmployeeStatistics = e.EmployeeStatistics != null ? _employeeStatisticsService.MapSingleEntityToDto(e.EmployeeStatistics) : null,
                Address = e.Address != null ? _addressesService.MapSingleEntityToDto(e.Address) : null,
                EmployeePosition = e.EmployeePosition != null ? _employeePositionsService.MapSingleEntityToDto(e.EmployeePosition) : null,
                RentalPlace = e.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(e.RentalPlace) : null,
                Supervisor = e.Supervisor != null ? new EmployeeSelectorDto
                {
                    Id = e.Supervisor.Id,
                    FirstName = e.Supervisor.FirstName,
                    LastName = e.Supervisor.LastName,
                } : null
            };
        }

        public override EmployeeDto MapSingleEntityToDto(Employee entity)
        {
            return new EmployeeDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                RentalPlaceId = entity.RentalPlaceId,
                AddressId = entity.AddressId,
                EmployeePositionId = entity.EmployeePositionId,
                SupervisorId = entity.SupervisorId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth,
                HireDate = entity.HireDate,
                TerminationDate = entity.TerminationDate,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,
                EmployeeStatisticsId = entity.EmployeeStatisticsId,
                EmployeeFinancesId = entity.EmployeeFinancesId,
                EmployeeFinances = entity.EmployeeFinances != null ? _employeeFinancesService.MapSingleEntityToDto(entity.EmployeeFinances) : null,
                EmployeeStatistics = entity.EmployeeStatistics != null ? _employeeStatisticsService.MapSingleEntityToDto(entity.EmployeeStatistics) : null,
                Address = entity.Address != null ? _addressesService.MapSingleEntityToDto(entity.Address) : null,
                EmployeePosition = entity.EmployeePosition != null ? _employeePositionsService.MapSingleEntityToDto(entity.EmployeePosition) : null,
                RentalPlace = entity.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(entity.RentalPlace) : null,
                Supervisor = entity.Supervisor != null ? new EmployeeSelectorDto
                {
                    Id = entity.Supervisor.Id,
                    FirstName = entity.Supervisor.FirstName,
                    LastName = entity.Supervisor.LastName,
                } : null
            };
        }

        #endregion

        public override async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(Employee).Name} not found");

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Employee entity, EmployeeDto model)
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.DateOfBirth = model.DateOfBirth;
            entity.PhoneNumber = model.PhoneNumber;
            entity.HireDate = model.HireDate;
            entity.TerminationDate = model.TerminationDate;
            entity.Status = model.Status;
            entity.Email = model.Email;
            // entity.UserName = model.UserName;

            // Update navigation properties if provided
            if (model.EmployeePosition != null)
            {
                entity.EmployeePositionId = model.EmployeePosition.EmployeePositionId;
            }

            if (model.RentalPlace != null)
            {
                entity.RentalPlaceId = model.RentalPlace.RentalPlaceId;
            }

            if (model.Supervisor != null)
            {
                entity.SupervisorId = model.Supervisor.Id;
            }

            if (model.Address != null)
            {
                entity.Address.FirstLine = model.Address.FirstLine;
                entity.Address.SecondLine = model.Address.SecondLine;
                entity.Address.ZipCode = model.Address.ZipCode;
                entity.Address.City = model.Address.City;

                entity.Address.IsActive = model.IsActive;
                entity.Address.DeletedDate = model.IsActive ? null : model.DeletedDate;
                entity.Address.ModifiedDate = DateTime.UtcNow;

                if (model.Address.Country != null)
                {
                    entity.Address.CountryId = model.Address.Country.CountryId;
                }
            }

            // Update EmployeeFinances if provided
            if (model.EmployeeFinances != null)
            {
                entity.EmployeeFinances.BaseSalary = model.EmployeeFinances.BaseSalary ?? model.EmployeePosition.DefaultBaseSalary;
                entity.EmployeeFinances.HourlyRate = model.EmployeeFinances.HourlyRate ?? model.EmployeePosition.DefaultHourlyRate;
                entity.EmployeeFinances.ModifiedDate = DateTime.UtcNow;
            }

            // Restore the entity
            if (model.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        public override async Task<Employee> FindEntityById(int id)
        {
            return await _context.Employees
                .Include(e => e.Address)
                .Include(e => e.Address.Country)
                .Include(e => e.EmployeePosition)
                .Include(e => e.RentalPlace)
                .Include(e => e.RentalPlace.Address)
                .Include(e => e.RentalPlace.Address.Country)
                .Include(e => e.RentalPlace.Location)
                .Include(e => e.Supervisor)
                .Include(e => e.EmployeeStatistics)
                .Include(e => e.EmployeeFinances)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        protected override IQueryable<Employee> IncludeRelatedEntities(IQueryable<Employee> query)
        {
            return query
                .Include(e => e.Address)
                .Include(e => e.Address.Country)
                .Include(e => e.EmployeePosition)
                .Include(e => e.RentalPlace)
                .Include(e => e.RentalPlace.Address)
                .Include(e => e.RentalPlace.Address.Country)
                .Include(e => e.RentalPlace.Location)
                .Include(e => e.Supervisor)
                .Include(e => e.EmployeeStatistics)
                .Include(e => e.EmployeeFinances);
        }
        public override async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate the Address in the DTO
                if (employeeDto.Address == null)
                {
                    throw new ArgumentException("Address is required.");
                }

                // Create and save the new Address entity
                var newAddress = new Address
                {
                    FirstLine = employeeDto.Address.FirstLine,
                    City = employeeDto.Address.City,
                    SecondLine = employeeDto.Address.SecondLine,
                    ZipCode = employeeDto.Address.ZipCode,
                    CountryId = employeeDto.Address.Country.CountryId, // Use existing CountryId
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync(); // Save the new Address to generate an AddressId

                // Set the default statistics if not provided
                var employeeStatisticsDto = new EmployeeStatisticsDto
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

                // Set the default base salary and hourly rate if not provided
                var employeeFinancesDto = new EmployeeFinanceDto
                {
                    BaseSalary = employeeDto.EmployeeFinances.BaseSalary ?? employeeDto.EmployeePosition.DefaultBaseSalary,
                    HourlyRate = employeeDto.EmployeeFinances.HourlyRate ?? employeeDto.EmployeePosition.DefaultHourlyRate,
                    ModifiedDate = DateTime.Now
                };

                // Save the related entities first
                var createdStatistics = await _employeeStatisticsService.CreateAsync(employeeStatisticsDto);
                var createdFinances = await _employeeFinancesService.CreateAsync(employeeFinancesDto);
                await _context.SaveChangesAsync();

                // Generate a unique username
                var generatedUserName = await GenerateUniqueUsernameAsync(employeeDto.FirstName, employeeDto.LastName);

                // Create the employee entity
                var employee = new Employee
                {
                    UserName = generatedUserName,
                    Email = employeeDto.Email,
                    PhoneNumber = employeeDto.PhoneNumber,
                    RentalPlaceId = employeeDto.RentalPlace?.RentalPlaceId ?? throw new ArgumentException("RentalPlace is required."),
                    AddressId = newAddress.AddressId, // Use the newly generated AddressId
                    EmployeePositionId = employeeDto.EmployeePosition?.EmployeePositionId ?? throw new ArgumentException("EmployeePosition is required."),
                    SupervisorId = employeeDto.Supervisor?.Id,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    DateOfBirth = employeeDto.DateOfBirth,
                    HireDate = employeeDto.HireDate,
                    TerminationDate = employeeDto.TerminationDate,
                    Status = employeeDto.Status,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    EmployeeStatisticsId = createdStatistics.EmployeeStatisticsId,
                    EmployeeFinancesId = createdFinances.EmployeeFinancesId
                };

                // Create the employee in the Identity system
                var result = await _userManager.CreateAsync(employee, employeeDto.Password);

                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Employee creation failed: {errorMessages}");
                }

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the created employee as a DTO
                return MapSingleEntityToDto(employee);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while creating the employee.", ex);
            }
        }

        //Custom method for soft deleting an employee
        public override async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get the employee entity with its related IDs
                var employee = await FindEntityById(id);
                if (employee == null)
                    return false;

                // Delete employee using base implementation
                var employeeDeleted = await base.DeleteAsync(id);
                if (!employeeDeleted)
                    return false;

                // Delete associated address
                await _addressesService.DeleteAsync(employee.AddressId);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateUniqueUsernameAsync(string firstName, string lastName)
        {
            var rnd = new Random();
            var baseUsername = $"{firstName.ToLower()}.{lastName.ToLower()}";
            var generatedUsername = $"{baseUsername}{rnd.Next(1000):000}";

            // Ensure the username is unique
            while (await _context.Employees.AnyAsync(e => e.UserName == generatedUsername))
            {
                generatedUsername = $"{baseUsername}{rnd.Next(1000):000}";
            }

            return generatedUsername;
        }
    }
}

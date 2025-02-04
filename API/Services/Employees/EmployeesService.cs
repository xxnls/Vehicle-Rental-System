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
                e.UserName.Contains(search);
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
                Supervisor = e.Supervisor != null ? new EmployeeDto
                {
                    Id = e.Supervisor.Id,
                    UserName = e.Supervisor.UserName,
                    Email = e.Supervisor.Email,
                    PhoneNumber = e.Supervisor.PhoneNumber,
                    FirstName = e.Supervisor.FirstName,
                    LastName = e.Supervisor.LastName,
                    DateOfBirth = e.Supervisor.DateOfBirth,
                    HireDate = e.Supervisor.HireDate,
                    TerminationDate = e.Supervisor.TerminationDate,
                    Status = e.Supervisor.Status,
                    CreatedDate = e.Supervisor.CreatedDate,
                    ModifiedDate = e.Supervisor.ModifiedDate,
                    DeletedDate = e.Supervisor.DeletedDate,
                    IsActive = e.Supervisor.IsActive
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
                Supervisor = entity.Supervisor != null ? new EmployeeDto
                {
                    Id = entity.Supervisor.Id,
                    UserName = entity.Supervisor.UserName,
                    Email = entity.Supervisor.Email,
                    PhoneNumber = entity.Supervisor.PhoneNumber,
                    FirstName = entity.Supervisor.FirstName,
                    LastName = entity.Supervisor.LastName,
                    DateOfBirth = entity.Supervisor.DateOfBirth,
                    HireDate = entity.Supervisor.HireDate,
                    TerminationDate = entity.Supervisor.TerminationDate,
                    Status = entity.Supervisor.Status,
                    CreatedDate = entity.Supervisor.CreatedDate,
                    ModifiedDate = entity.Supervisor.ModifiedDate,
                    DeletedDate = entity.Supervisor.DeletedDate,
                    IsActive = entity.Supervisor.IsActive
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
                entity.SupervisorId = model.Supervisor.SupervisorId;
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

            // Update EmployeeStatistics if provided
            //if (model.EmployeeStatistics != null)
            //{
            //    entity.EmployeeStatistics.TotalWorkDays = model.EmployeeStatistics.TotalWorkDays;
            //    entity.EmployeeStatistics.LateArrivals = model.EmployeeStatistics.LateArrivals;
            //    entity.EmployeeStatistics.EarlyDepartures = model.EmployeeStatistics.EarlyDepartures;
            //    entity.EmployeeStatistics.OvertimeHours = model.EmployeeStatistics.OvertimeHours;
            //    entity.EmployeeStatistics.SickLeavesTaken = model.EmployeeStatistics.SickLeavesTaken;
            //    entity.EmployeeStatistics.VacationDaysTaken = model.EmployeeStatistics.VacationDaysTaken;
            //    entity.EmployeeStatistics.UnpaidLeavesTaken = model.EmployeeStatistics.UnpaidLeavesTaken;
            //    entity.EmployeeStatistics.TotalRentalsApproved = model.EmployeeStatistics.TotalRentalsApproved;
            //}

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

        // Custom method for creating an employee
        public override async Task<EmployeeDto> CreateAsync(EmployeeDto employeeDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                    AddressId = employeeDto.Address?.AddressId ?? throw new ArgumentException("Address is required."),
                    EmployeePositionId = employeeDto.EmployeePosition?.EmployeePositionId ?? throw new ArgumentException("EmployeePosition is required."),
                    SupervisorId = employeeDto.Supervisor?.SupervisorId,
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

        // Custom method for soft deleting an employee
        //public override async Task<bool> DeleteAsync(int id)
        //{
        //    var employee = await FindEntityById(id);
        //    if (employee == null)
        //        return false;

        //    // Set DeletedDate and IsActive for soft delete
        //    employee.DeletedDate = DateTime.UtcNow;
        //    employee.IsActive = false;
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

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

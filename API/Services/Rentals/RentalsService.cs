using API.Context;
using API.Models;
using API.Models.DTOs;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Vehicles;
using API.Models.DTOs.Rentals;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.BusinessLogic;
using API.Models.Rentals;
using API.Services.Customers;
using API.Services.Employees;
using API.Services.Vehicles;
using Microsoft.EntityFrameworkCore.Storage;
using DepositStatus = API.Models.Rentals.DepositStatus;
using RentalStatus = API.Models.DTOs.Rentals.RentalStatus;
using PaymentStatus = API.Models.DTOs.Rentals.PaymentStatus;

namespace API.Services.Rentals
{
    public class RentalsService : BaseApiService<Rental, RentalDto, RentalDto>
    {
        private readonly ApiDbContext _context;
        private readonly CustomersService _customersService;
        private readonly VehiclesService _vehiclesService;
        private readonly EmployeesService _employeesService;
        private readonly IRentalCostCalculator _rentalCostCalculator;

        public RentalsService(
            ApiDbContext context,
            CustomersService customersService,
            VehiclesService vehiclesService,
            EmployeesService employeesService,
            IRentalCostCalculator rentalCostCalculator) : base(context)
        {
            _context = context;
            _customersService = customersService;
            _vehiclesService = vehiclesService;
            _employeesService = employeesService;
            _rentalCostCalculator = rentalCostCalculator;
        }

        protected override Expression<Func<Rental, bool>> BuildSearchQuery(string search)
        {
            return r =>
                r.RentalId.ToString().Contains(search) ||
                r.Customer.FirstName.Contains(search) ||
                r.Customer.LastName.Contains(search) ||
                r.Vehicle.VehicleModel.Name.Contains(search) ||
                r.Vehicle.VehicleModel.VehicleBrand.Name.Contains(search) ||
                r.StartDate.ToString().Contains(search) ||
                r.EndDate.ToString().Contains(search) ||
                r.RentalStatus.Contains(search) ||
                r.PaymentStatus.Contains(search) ||
                r.DepositStatus.Contains(search) ||
                r.DepositAmount.ToString().Contains(search) ||
                r.DepositRefundAmount.ToString().Contains(search) ||
                r.DamageFee.ToString().Contains(search) ||
                r.Cost.ToString().Contains(search);
        }

        protected override Expression<Func<Rental, bool>> GetActiveFilter(bool showDeleted)
        {
            return r => r.IsActive != showDeleted;
        }

        #region Mapping

        public override Rental MapToEntity(RentalDto model)
        {
            return new Rental
            {
                RentalId = model.RentalId,
                CustomerId = model.CustomerId,
                VehicleId = model.VehicleId,
                StartedByEmployeeId = model.StartedByEmployeeId,
                FinishedByEmployeeId = model.FinishedByEmployeeId,
                RentalStatus = model.RentalStatus,
                PaymentStatus = model.PaymentStatus,
                DepositStatus = model.DepositStatus,
                DamageFeePaymentStatus = model.DamageFeePaymentStatus,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PickupDateTime = model.PickupDateTime,
                FinishDateTime = model.FinishDateTime,
                DepositAmount = model.DepositAmount,
                DepositRefundAmount = model.DepositRefundAmount,
                Cost = model.Cost,
                FinalCost = model.FinalCost,
                DamageFee = model.DamageFee,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate
            };
        }

        public override Expression<Func<Rental, RentalDto>> MapToDto()
        {
            return r => new RentalDto
            {
                RentalId = r.RentalId,
                CustomerId = r.CustomerId,
                Customer = r.Customer != null ? _customersService.MapSingleEntityToDto(r.Customer) : null,
                VehicleId = r.VehicleId,
                Vehicle = r.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(r.Vehicle) : null,
                StartedByEmployeeId = r.StartedByEmployeeId,
                StartedByEmployee = r.StartedByEmployee != null ? _employeesService.MapSingleEntityToDto(r.StartedByEmployee) : null,
                FinishedByEmployeeId = r.FinishedByEmployeeId,
                FinishedByEmployee = r.FinishedByEmployee != null ? _employeesService.MapSingleEntityToDto(r.FinishedByEmployee) : null,
                RentalStatus = r.RentalStatus,
                PaymentStatus = r.PaymentStatus,
                DepositStatus = r.DepositStatus,
                DamageFeePaymentStatus = r.DamageFeePaymentStatus,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                PickupDateTime = r.PickupDateTime,
                FinishDateTime = r.FinishDateTime,
                DepositAmount = r.DepositAmount,
                DepositRefundAmount = r.DepositRefundAmount,
                Cost = r.Cost,
                FinalCost = r.FinalCost,
                DamageFee = r.DamageFee,
                IsActive = r.IsActive,
                CreatedDate = r.CreatedDate,
                ModifiedDate = r.ModifiedDate,
                DeletedDate = r.DeletedDate
            };
        }

        public override RentalDto MapSingleEntityToDto(Rental entity)
        {
            return new RentalDto
            {
                RentalId = entity.RentalId,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer != null ? _customersService.MapSingleEntityToDto(entity.Customer) : null,
                VehicleId = entity.VehicleId,
                Vehicle = entity.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(entity.Vehicle) : null,
                StartedByEmployeeId = entity.StartedByEmployeeId,
                StartedByEmployee = entity.StartedByEmployee != null ? _employeesService.MapSingleEntityToDto(entity.StartedByEmployee) : null,
                FinishedByEmployeeId = entity.FinishedByEmployeeId,
                FinishedByEmployee = entity.FinishedByEmployee != null ? _employeesService.MapSingleEntityToDto(entity.FinishedByEmployee) : null,
                RentalStatus = entity.RentalStatus,
                PaymentStatus = entity.PaymentStatus,
                DepositStatus = entity.DepositStatus,
                DamageFeePaymentStatus = entity.DamageFeePaymentStatus,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                PickupDateTime = entity.PickupDateTime,
                FinishDateTime = entity.FinishDateTime,
                DepositAmount = entity.DepositAmount,
                DepositRefundAmount = entity.DepositRefundAmount,
                Cost = entity.Cost,
                FinalCost = entity.FinalCost,
                DamageFee = entity.DamageFee,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate
            };
        }

        #endregion

        public override async Task<RentalDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(Rental).Name} not found");

            return MapSingleEntityToDto(entity);
        }

        public async Task<PaginatedResult<RentalDto>> GetAwaitingRentalsAsync(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            var query = _context.Rentals
                .Where(r => r.RentalStatus == RentalStatus.AwaitingPickup.ToString() && r.IsActive);

            return await GetAllAsync(
                search, page, false, createdBefore, createdAfter, modifiedBefore, modifiedAfter, pageSize, query);
        }

        public async Task<PaginatedResult<RentalDto>> GetInProgressRentalsAsync(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            var query = _context.Rentals
                .Where(r => r.RentalStatus == RentalStatus.InProgress.ToString() && r.IsActive);

            return await GetAllAsync(
                search, page, false, createdBefore, createdAfter, modifiedBefore, modifiedAfter, pageSize, query);
        }

        protected override void UpdateEntity(Rental entity, RentalDto model)
        {
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.PickupDateTime = model.PickupDateTime;
            entity.FinishDateTime = model.FinishDateTime;
            entity.DepositAmount = model.DepositAmount;
            entity.DepositRefundAmount = model.DepositRefundAmount;
            entity.Cost = model.Cost;
            entity.FinalCost = model.FinalCost;
            entity.DamageFee = model.DamageFee;
            entity.RentalStatus = model.RentalStatus;
            entity.PaymentStatus = model.PaymentStatus;
            entity.DepositStatus = model.DepositStatus;
            entity.DamageFeePaymentStatus = model.DamageFeePaymentStatus;

            // Update navigation properties if provided
            if (model.Customer != null)
            {
                entity.CustomerId = model.Customer.Id;
            }

            if (model.Vehicle != null)
            {
                entity.VehicleId = model.Vehicle.VehicleId;
            }

            if (model.StartedByEmployee != null)
            {
                entity.StartedByEmployeeId = model.StartedByEmployee.Id;
            }

            if (model.FinishedByEmployee != null)
            {
                entity.FinishedByEmployeeId = model.FinishedByEmployee.Id;
            }

            if (model.PostRentalReport != null)
            {
                entity.PostRentalReportId = model.PostRentalReport.PostRentalReportId;
            }

            // Restore the entity
            if (model.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        public override async Task<Rental> FindEntityById(int id)
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.Vehicle)
                .Include(r => r.Vehicle.VehicleStatus)
                .Include(r => r.Vehicle.VehicleType)
                .Include(r => r.Vehicle.VehicleModel)
                .Include(r => r.Vehicle.VehicleModel.VehicleBrand)
                .Include(r => r.StartedByEmployee)
                .Include(r => r.FinishedByEmployee)
                .FirstOrDefaultAsync(r => r.RentalId == id);
        }

        protected override IQueryable<Rental> IncludeRelatedEntities(IQueryable<Rental> query)
        {
            return query
                .Include(r => r.Customer)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.Vehicle)
                .Include(r => r.Vehicle.VehicleStatus)
                .Include(r => r.Vehicle.VehicleType)
                .Include(r => r.Vehicle.VehicleModel)
                .Include(r => r.Vehicle.VehicleModel.VehicleBrand)
                .Include(r => r.StartedByEmployee)
                .Include(r => r.FinishedByEmployee);
        }

        public override async Task<RentalDto> CreateAsync(RentalDto rentalDto)
        {
            try
            {
                // Use calculator again to avoid mistakes
                rentalDto.Cost = await _rentalCostCalculator.Calculate(rentalDto);

                var rental = new Rental
                {
                    CustomerId = rentalDto.Customer.Id,
                    VehicleId = rentalDto.Vehicle.VehicleId,
                    StartedByEmployeeId = rentalDto.StartedByEmployee.Id,
                    RentalStatus = RentalStatus.AwaitingPickup.ToString(), // Set initial status
                    PaymentStatus = PaymentStatus.Pending.ToString(), // Set initial status
                    DepositStatus = DepositStatus.Pending.ToString(), // Set initial status
                    StartDate = rentalDto.StartDate,
                    EndDate = rentalDto.EndDate,
                    Cost = rentalDto.Cost,
                    DepositAmount = rentalDto.DepositAmount,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                };

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                return MapSingleEntityToDto(rental);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the rental.", ex);
            }
        }

        // Custom method for soft deleting a rental
        public override async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rental = await FindEntityById(id);
                if (rental == null)
                    return false;

                var rentalDeleted = await base.DeleteAsync(id);
                if (!rentalDeleted)
                    return false;

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
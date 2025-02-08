using API.Context;
using API.Models;
using API.Models.DTOs;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Vehicles;
using API.Models.DTOs.Rentals;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.BusinessLogic;
using API.Models.Rentals;
using API.Services.Customers;
using API.Services.Vehicles;
using RentalRequestStatus = API.Models.Rentals.RentalRequestStatus;
using PaymentStatus = API.Models.Rentals.PaymentStatus;

namespace API.Services.Rentals
{
    public class RentalRequestsService : BaseApiService<RentalRequest, RentalRequestDto, RentalRequestDto>
    {
        private readonly ApiDbContext _context;
        private readonly CustomersService _customersService;
        private readonly VehiclesService _vehiclesService;
        private readonly IRentalCostCalculator _rentalCostCalculator;

        public RentalRequestsService(
            ApiDbContext context,
            CustomersService customersService,
            VehiclesService vehiclesService,
            IRentalCostCalculator rentalCostCalculator) : base(context)
        {
            _context = context;
            _customersService = customersService;
            _vehiclesService = vehiclesService;
            _rentalCostCalculator = rentalCostCalculator;
        }

        protected override Expression<Func<RentalRequest, bool>> BuildSearchQuery(string search)
        {
            return r =>
                r.RentalRequestId.ToString().Contains(search) ||
                r.Customer.FirstName.Contains(search) ||
                r.Customer.LastName.Contains(search) ||
                r.Vehicle.VehicleModel.Name.Contains(search) ||
                r.Vehicle.VehicleModel.VehicleBrand.Name.Contains(search) ||
                r.RequestDate.ToString().Contains(search) ||
                r.StartDate.ToString().Contains(search) ||
                r.EndDate.ToString().Contains(search) ||
                r.RequestStatus.ToString().Contains(search) ||
                r.PaymentStatus.ToString().Contains(search) ||
                r.Notes.Contains(search);
        }

        protected override Expression<Func<RentalRequest, bool>> GetActiveFilter(bool showDeleted)
        {
            return v => v.IsActive != showDeleted;
        }

        #region Mapping

        public override RentalRequest MapToEntity(RentalRequestDto model)
        {
            return new RentalRequest
            {
                RentalRequestId = model.RentalRequestId,
                CustomerId = model.CustomerId,
                VehicleId = model.VehicleId,
                RequestDate = model.RequestDate,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TotalCost = model.TotalCost,
                RequestStatus = model.RequestStatus,
                PaymentStatus = model.PaymentStatus,
                Notes = model.Notes,
                IsActive = model.IsActive,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate
            };
        }

        public override Expression<Func<RentalRequest, RentalRequestDto>> MapToDto()
        {
            return r => new RentalRequestDto
            {
                RentalRequestId = r.RentalRequestId,
                CustomerId = r.CustomerId,
                Customer = r.Customer != null ? _customersService.MapSingleEntityToDto(r.Customer) : null,
                VehicleId = r.VehicleId,
                Vehicle = r.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(r.Vehicle) : null,
                RequestDate = r.RequestDate,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalCost = r.TotalCost,
                RequestStatus = r.RequestStatus,
                PaymentStatus = r.PaymentStatus,
                Notes = r.Notes,
                IsActive = r.IsActive,
                CreatedDate = r.CreatedDate,
                ModifiedDate = r.ModifiedDate,
                DeletedDate = r.DeletedDate
            };
        }

        public override RentalRequestDto MapSingleEntityToDto(RentalRequest entity)
        {
            return new RentalRequestDto
            {
                RentalRequestId = entity.RentalRequestId,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer != null ? _customersService.MapSingleEntityToDto(entity.Customer) : null,
                VehicleId = entity.VehicleId,
                Vehicle = entity.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(entity.Vehicle) : null,
                RequestDate = entity.RequestDate,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                TotalCost = entity.TotalCost,
                RequestStatus = entity.RequestStatus,
                PaymentStatus = entity.PaymentStatus,
                Notes = entity.Notes,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate
            };
        }

        #endregion

        public override async Task<RentalRequestDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(RentalRequest).Name} not found");

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(RentalRequest entity, RentalRequestDto model)
        {
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.TotalCost = model.TotalCost;
            entity.RequestStatus = model.RequestStatus;
            entity.PaymentStatus = model.PaymentStatus;
            entity.Notes = model.Notes;

            // Update navigation properties if provided
            if (model.Customer != null)
            { 
                entity.CustomerId = model.Customer.Id;
            }

            if (model.Vehicle != null)
            {
                entity.VehicleId = model.Vehicle.VehicleId;
            }

            // Restore the entity
            if (model.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        public override async Task<RentalRequest> FindEntityById(int id)
        {
            return await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.Vehicle)
                .Include(r => r.Vehicle.VehicleType)
                .Include(r => r.Vehicle.VehicleModel)
                .Include(r => r.Vehicle.VehicleModel.VehicleBrand)
                .FirstOrDefaultAsync(r => r.RentalRequestId == id);
        }

        protected override IQueryable<RentalRequest> IncludeRelatedEntities(IQueryable<RentalRequest> query)
        {
            return query
                .Include(r => r.Customer)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.Vehicle)
                .Include(r => r.Vehicle.VehicleType)
                .Include(r => r.Vehicle.VehicleModel)
                .Include(r => r.Vehicle.VehicleModel.VehicleBrand);
        }

        public override async Task<RentalRequestDto> CreateAsync(RentalRequestDto rentalRequestDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Use calculator
                rentalRequestDto.TotalCost = await _rentalCostCalculator.Calculate(rentalRequestDto);

                // Create the rental request entity
                var rentalRequest = new RentalRequest
                {
                    CustomerId = rentalRequestDto.Customer.Id,
                    VehicleId = rentalRequestDto.Vehicle.VehicleId,
                    RequestDate = DateTime.UtcNow,
                    StartDate = rentalRequestDto.StartDate,
                    EndDate = rentalRequestDto.EndDate,
                    TotalCost = rentalRequestDto.TotalCost,
                    RequestStatus = RentalRequestStatus.Pending.ToString(), // Set RequestStatus to Pending
                    PaymentStatus = PaymentStatus.Pending.ToString(),       // Set PaymentStatus to Pending
                    Notes = rentalRequestDto.Notes,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                _context.RentalRequests.Add(rentalRequest);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapSingleEntityToDto(rentalRequest);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while creating the rental request.", ex);
            }
        }

        // Custom method for soft deleting a rental request
        public override async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rentalRequest = await FindEntityById(id);
                if (rentalRequest == null)
                    return false;

                var rentalRequestDeleted = await base.DeleteAsync(id);
                if (!rentalRequestDeleted)
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
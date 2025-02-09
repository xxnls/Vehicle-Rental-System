using API.Context;
using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using API.Services.Vehicles;
using Microsoft.EntityFrameworkCore;
using RentalRequestStatus = API.Models.Rentals.RentalRequestStatus;

namespace API.BusinessLogic
{
    public class RentalProcessing : IRentalProcessing
    {
        private readonly RentalRequestsService _rentalRequestsService;
        private readonly VehiclesService _vehiclesService;
        private readonly VehicleStatusesService _vehicleStatusesService;
        // private readonly RentalsService _rentalRepository;
        private readonly ApiDbContext _context;

        public RentalProcessing(
            RentalRequestsService rentalRequestsService,
            VehiclesService vehiclesService,
            VehicleStatusesService vehicleStatusesService,
            // RentalsService rentalRepository,
            ApiDbContext context)
        {
            _rentalRequestsService = rentalRequestsService;
            _vehiclesService = vehiclesService;
            _vehicleStatusesService = vehicleStatusesService;
            // _rentalRepository = rentalRepository;
            _context = context;
        }

        public async Task<bool> ApproveRentalRequestAsync(int rentalRequestId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rentalRequest = await _rentalRequestsService.GetByIdAsync(rentalRequestId);
                if (rentalRequest == null)
                {
                    throw new ArgumentNullException(nameof(rentalRequest), "Rental request not found.");
                }

                if (rentalRequest.RequestStatus != RentalRequestStatus.Pending.ToString())
                {
                    throw new InvalidOperationException("Rental request is not in pending status.");
                }

                if (!await IsVehicleAvailableAsync(rentalRequest.VehicleId))
                {
                    throw new InvalidOperationException("Vehicle is not available.");
                }

                rentalRequest.RequestStatus = RentalRequestStatus.Approved.ToString();
                await _rentalRequestsService.UpdateAsync(rentalRequestId, rentalRequest);

                // Change vehicle status to rented
                await ChangeVehicleStatusAsync(rentalRequest.VehicleId, 2);

                //var rental = new RentalDto
                //{
                //    VehicleId = rentalRequest.VehicleId,
                //    CustomerId = rentalRequest.CustomerId,
                //    StartDate = rentalRequest.StartDate,
                //    EndDate = rentalRequest.EndDate,
                //    // ... set other properties
                //};

                // TODO: ADDING RENTAL
                //await _context.Rentals.AddAsync(rental); // Add to the correct DbSet
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex) // Catch concurrency exceptions
            {
                await transaction.RollbackAsync();
                throw new Exception("A concurrency error occurred. Please try again."); // .NET exception
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw the original exception
            }
        }

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId)
        {
            var vehicle = await _vehiclesService.GetByIdAsync(vehicleId);
            return vehicle?.IsAvailableForRent ?? false;
        }

        public async Task ChangeVehicleStatusAsync(int vehicleId, int statusId)
        {
            var vehicle = await _vehiclesService.GetByIdAsync(vehicleId);

            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle not found.");
            }

            var statusName = await _vehicleStatusesService.GetByIdAsync(statusId).ConfigureAwait(false);

            vehicle.IsAvailableForRent = statusName.StatusName switch
            {
                "Available" => true,
                "Rented" or "Maintenance" or "UnderInspection" or "OutOfService" => false,
                null => throw new ArgumentNullException(nameof(statusId), "Vehicle status not found."),
                _ => vehicle.IsAvailableForRent
            };

            vehicle.VehicleStatus.VehicleStatusId = statusId;
            await _vehiclesService.UpdateAsync(vehicleId, vehicle);
        }
    }

    public interface IRentalProcessing
    {
        Task<bool> ApproveRentalRequestAsync(int rentalRequestId);
        Task<bool> IsVehicleAvailableAsync(int vehicleId);
        Task ChangeVehicleStatusAsync(int vehicleId, int statusId);
    }
}

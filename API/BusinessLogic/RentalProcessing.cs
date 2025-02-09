﻿using API.Context;
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
        private readonly RentalsService _rentalService;
        private readonly ApiDbContext _context;

        public RentalProcessing(
            RentalRequestsService rentalRequestsService,
            VehiclesService vehiclesService,
            VehicleStatusesService vehicleStatusesService,
            RentalsService rentalService,
            ApiDbContext context)
        {
            _rentalRequestsService = rentalRequestsService;
            _vehiclesService = vehiclesService;
            _vehicleStatusesService = vehicleStatusesService;
            _rentalService = rentalService;
            _context = context;
        }

        public async Task<bool> ApproveRentalRequestAsync(RentalRequestDto rentalRequest)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                await _rentalRequestsService.UpdateAsync(rentalRequest.RentalRequestId, rentalRequest);

                // Change vehicle status to rented
                await ChangeVehicleStatusAsync(rentalRequest.VehicleId, 2);

                var rental = new RentalDto
                {
                    Vehicle = rentalRequest.Vehicle,
                    Customer = rentalRequest.Customer,
                    StartedByEmployee = rentalRequest.ModifiedByEmployee,
                    StartDate = rentalRequest.StartDate,
                    EndDate = rentalRequest.EndDate,

                    // Check if the custom deposit is set, otherwise set it to the base deposit of type
                    DepositAmount = rentalRequest.Vehicle.CustomDeposit ?? rentalRequest.Vehicle.VehicleType.BaseDeposit,
                };

                await _rentalService.CreateAsync(rental);

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
        Task<bool> ApproveRentalRequestAsync(RentalRequestDto rentalRequest);
        Task<bool> IsVehicleAvailableAsync(int vehicleId);
        Task ChangeVehicleStatusAsync(int vehicleId, int statusId);
    }
}

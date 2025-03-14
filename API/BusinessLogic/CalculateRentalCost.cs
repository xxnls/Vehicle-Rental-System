﻿using API.Interfaces;
using API.Models.DTOs.Rentals;
using API.Models.Vehicles;
using API.Services.Vehicles;
using Microsoft.Extensions.Configuration;

namespace API.BusinessLogic
{
    public class RentalCostCalculator(IConfiguration configuration, VehiclesService vehiclesService)
        : IRentalCostCalculator
    {
        /// <summary>
        /// Calculate the total cost of a rental request.
        /// </summary>
        /// <param name="rental">
        /// The rental request DTO.
        /// </param>
        /// <returns>
        /// The total cost of the rental request.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the rental duration is less than the minimum rental days, the vehicle is not found,
        /// the vehicle type or base daily rate is missing,or the customer and vehicle are required.
        /// </exception>
        public async Task<decimal> Calculate(IRentalCostCalculation rental)
        {
            var countInclusive = configuration.GetValue<bool>("RentalSettings:CountRentalDayInclusive");
            var minimumRentalDays = configuration.GetValue<int>("RentalSettings:MinimumRentalDays");

            // Validate the Customer and Vehicle in the rental request
            if (rental.Customer == null || rental.Vehicle == null)
            {
                throw new ArgumentException("Customer and Vehicle are required.");
            }

            // Get the fresh vehicle data from the database
            var vehicle = await vehiclesService.GetByIdAsync(rental.Vehicle.VehicleId);

            // If vehicle is not found, throw an exception
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found.");
            }

            // Calculate rental duration
            var rentalDuration = (rental.EndDate - rental.StartDate).TotalDays;

            // If countInclusive is true, add 1 to the rental duration
            if (countInclusive)
                rentalDuration++;

            // If rental duration is less than the minimum rental days, throw an exception
            if (rentalDuration < minimumRentalDays)
                throw new ArgumentException($"Rental duration must be at least {minimumRentalDays} day/s.");

            decimal totalCost;

            // If custom daily rate is set, use it, otherwise use the base daily rate
            if (vehicle.CustomDailyRate.HasValue)
            {
                totalCost = (decimal)rentalDuration * vehicle.CustomDailyRate.Value;
            }
            else
            {
                totalCost = (decimal)rentalDuration * vehicle.VehicleType.BaseDailyRate;
            }

            // If total cost is 0, throw an exception
            if (totalCost == 0)
                throw new ArgumentException("Vehicle type or base daily rate is missing.");

            // Apply customer discount
            if (rental.Customer.CustomerType.DiscountPercent != null)
                totalCost -= (totalCost * ((decimal)rental.Customer.CustomerType.DiscountPercent / 100));

            return totalCost;
        }
    }

    public interface IRentalCostCalculator
    { 
        Task<decimal> Calculate(IRentalCostCalculation rental);
    }
}
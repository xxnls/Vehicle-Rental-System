using API.Context;
using API.Models.Vehicles;

namespace API.Seeders
{
    public class DictionaryTablesSeeder(ApiDbContext context)
    {
        private readonly ApiDbContext _context = context;

        public static async Task SeedAsync(ApiDbContext context)
        {
            await SeedVehicleStatusesAsync(context);
        }

        private static async Task SeedVehicleStatusesAsync(ApiDbContext context)
        {
            var vehicleStatuses = new List<VehicleStatus>
            {
                new() { StatusName = "Available", Description = "Vehicle is available for rent." },
                new() { StatusName = "Rented", Description = "Vehicle is currently rented." },
                new() { StatusName = "Maintenance", Description = "Vehicle is undergoing maintenance." },
                new() { StatusName = "InService", Description = "Vehicle is in service." },
                new() { StatusName = "OutOfService", Description = "Vehicle is temporarily out of service." }
            };

            // Check if ANY VehicleStatus exists before seeding
            if (!context.VehicleStatuses.Any())
            {
                await context.VehicleStatuses.AddRangeAsync(vehicleStatuses);
                await context.SaveChangesAsync();
            }
        }
    }
}

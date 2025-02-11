using API.Context;
using API.Models.FileSystem;
using API.Models.Vehicles;

namespace API.Seeders
{
    public class DictionaryTablesSeeder(ApiDbContext context)
    {
        private readonly ApiDbContext _context = context;

        public static async Task SeedAsync(ApiDbContext context)
        {
            await SeedVehicleStatusesAsync(context);
            await SeedDocumentTypesAsync(context);
            await SeedDocumentCategoriesAsync(context);
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

        private static async Task SeedDocumentTypesAsync(ApiDbContext context)
        {
            var documentTypes = new List<DocumentType>
            {
                new()
                {
                    Name = "Image",
                    Description = "Image files",
                    FileExtension = ".jpg,.jpeg,.png,.gif",
                    MaxFileSizeMb = 5,
                    IsActive = true
                },
                new()
                {
                    Name = "Document",
                    Description = "General documents, contracts, invoices etc.",
                    FileExtension = ".pdf,.docx,.doc",
                    MaxFileSizeMb = 10,
                    IsActive = true
                },
                new()
                {
                    Name = "Video",
                    Description = "Video files",
                    FileExtension = ".mp4,.avi",
                    MaxFileSizeMb = 50,
                    IsActive = true
                }
            };

            // Check if ANY DocumentType exists before seeding
            if (!context.DocumentTypes.Any())
            {
                await context.DocumentTypes.AddRangeAsync(documentTypes);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedDocumentCategoriesAsync(ApiDbContext context)
        {
            var documentCategories = new List<DocumentCategory>
            {
                new() { Name = "Employee", Description = "Employee related documents" },
                new() { Name = "Vehicle", Description = "Vehicle related documents" },
                new() { Name = "Customer", Description = "Customer related documents" },
                new() { Name = "Financial", Description = "Financial documents" },
                new() { Name = "Contracts", Description = "Contracts", ParentCategoryId = 1 },
                new() { Name = "Signed Contracts", Description = "Signed contracts", ParentCategoryId = 1 }
            };

            // Check if ANY DocumentCategory exists before seeding
            if (!context.DocumentCategories.Any())
            {
                await context.DocumentCategories.AddRangeAsync(documentCategories);
                await context.SaveChangesAsync();
            }
        }
    }
}

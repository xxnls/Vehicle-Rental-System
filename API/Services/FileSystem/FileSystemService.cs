using API.Context;
using API.Interfaces;
using API.Models.DTOs.FileSystem;
using API.Models.FileSystem;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.Models.Customers;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Other;
using API.Models.DTOs.Rentals;
using API.Models.DTOs.Vehicles;
using API.Services.Customers;
using API.Services.Employees;
using API.Services.Other;
using API.Services.Rentals;
using API.Services.Vehicles;

namespace API.Services.FileSystem
{
    public class FileSystemService : BaseApiService<Document, DocumentDto, DocumentDto>
    {
        private readonly EmployeesService _employeesService;
        private readonly CustomersService _customersService;
        private readonly RentalsService _rentalsService;
        private readonly VehiclesService _vehiclesService;
        private readonly DocumentCategoriesService _documentCategoriesService;
        private readonly DocumentTypesService _documentTypesService;
        private readonly RentalPlacesService _rentalPlacesService;

        public FileSystemService(ApiDbContext context, EmployeesService employeesService,
            CustomersService customersService, RentalsService rentalsService, VehiclesService vehiclesService,
            DocumentCategoriesService documentCategoriesService, DocumentTypesService documentTypesService,
            RentalPlacesService rentalPlacesService) : base(context)
        {
            _employeesService = employeesService;
            _customersService = customersService;
            _rentalsService = rentalsService;
            _vehiclesService = vehiclesService;
            _documentCategoriesService = documentCategoriesService;
            _documentTypesService = documentTypesService;
            _rentalPlacesService = rentalPlacesService;
        }

        protected override Expression<Func<Document, bool>> BuildSearchQuery(string search)
        {
            return document => document.Title.Contains(search) ||
                               document.Description.Contains(search) ||
                               document.DocumentId.ToString().Contains(search) ||
                               document.DocumentCategory.Name.Contains(search) ||
                               document.DocumentType.Name.Contains(search) ||
                               document.FileName.Contains(search) ||
                               document.OriginalFileName.Contains(search);
        }

        protected override Expression<Func<Document, bool>> GetActiveFilter(bool showDeleted)
        {
            return r => r.IsActive != showDeleted;
        }

        public override async Task<Document> FindEntityById(int id)
        {
            return await _dbSet
                .Include(d => d.CreatedByEmployee)
                .Include(d => d.Customer)
                .Include(d => d.Customer.Address)
                .Include(d => d.Customer.Address.Country)
                .Include(d => d.DocumentCategory)
                .Include(d => d.DocumentType)
                .Include(d => d.Employee)
                .Include(d => d.ModifiedByEmployee)
                .Include(d => d.Rental)
                .Include(d => d.RentalPlace)
                .Include(d => d.Vehicle)
                .FirstOrDefaultAsync(d => d.DocumentId == id);
        }

        protected override IQueryable<Document> IncludeRelatedEntities(IQueryable<Document> query)
        {
            return query
                .Include(d => d.CreatedByEmployee)
                .Include(d => d.Customer)
                .Include(d => d.Customer.Address)
                .Include(d => d.Customer.Address.Country)
                .Include(d => d.DocumentCategory)
                .Include(d => d.DocumentType)
                .Include(d => d.Employee)
                .Include(d => d.ModifiedByEmployee)
                .Include(d => d.Rental)
                .Include(d => d.RentalPlace)
                .Include(d => d.Vehicle);
        }

        public override Document MapToEntity(DocumentDto dto)
        {
            return new Document
            {
                DocumentTypeId = dto.DocumentTypeId,
                DocumentCategoryId = dto.DocumentCategoryId,
                VehicleId = dto.VehicleId,
                EmployeeId = dto.EmployeeId,
                CustomerId = dto.CustomerId,
                RentalPlaceId = dto.RentalPlaceId,
                RentalId = dto.RentalId,
                Title = dto.Title,
                Description = dto.Description,
                CreatedDate = dto.CreatedDate,
                CreatedByEmployeeId = dto.CreatedByEmployeeId,
                ModifiedDate = dto.ModifiedDate,
                ModifiedByEmployeeId = dto.ModifiedByEmployeeId,
                IsActive = dto.IsActive,
                DeletedDate = dto.DeletedDate
            };
        }

        public override Expression<Func<Document, DocumentDto>> MapToDto()
        {
            return document => new DocumentDto
            {
                DocumentId = document.DocumentId,
                DocumentTypeId = document.DocumentTypeId,
                DocumentCategoryId = document.DocumentCategoryId,
                VehicleId = document.VehicleId,
                EmployeeId = document.EmployeeId,
                CustomerId = document.CustomerId,
                RentalPlaceId = document.RentalPlaceId,
                RentalId = document.RentalId,
                Title = document.Title,
                Description = document.Description,
                FileName = document.FileName,
                OriginalFileName = document.OriginalFileName,
                FileSizeMb = document.FileSizeMb,
                CreatedDate = document.CreatedDate,
                ModifiedDate = document.ModifiedDate,
                DeletedDate = document.DeletedDate,
                CreatedByEmployeeId = document.CreatedByEmployeeId,
                ModifiedByEmployeeId = document.ModifiedByEmployeeId,
                IsActive = document.IsActive,
                CreatedByEmployee = document.CreatedByEmployee != null ? _employeesService.MapSingleEntityToDto(document.CreatedByEmployee) : null,
                Customer = document.Customer != null ? _customersService.MapSingleEntityToDto(document.Customer) : null,
                DocumentCategory = document.DocumentCategory != null ? _documentCategoriesService.MapSingleEntityToDto(document.DocumentCategory) : null,
                DocumentType = document.DocumentType != null ? _documentTypesService.MapSingleEntityToDto(document.DocumentType) : null,
                Employee = document.Employee != null ? _employeesService.MapSingleEntityToDto(document.Employee) : null,
                ModifiedByEmployee = document.ModifiedByEmployee != null ? _employeesService.MapSingleEntityToDto(document.ModifiedByEmployee) : null,
                Rental = document.Rental != null ? _rentalsService.MapSingleEntityToDto(document.Rental) : null,
                RentalPlace = document.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(document.RentalPlace) : null,
                Vehicle = document.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(document.Vehicle) : null
            };
        }

        public override DocumentDto MapSingleEntityToDto(Document entity)
        {
            return new DocumentDto
            {
                DocumentId = entity.DocumentId,
                DocumentTypeId = entity.DocumentTypeId,
                DocumentCategoryId = entity.DocumentCategoryId,
                VehicleId = entity.VehicleId,
                EmployeeId = entity.EmployeeId,
                CustomerId = entity.CustomerId,
                RentalPlaceId = entity.RentalPlaceId,
                RentalId = entity.RentalId,
                Title = entity.Title,
                Description = entity.Description,
                FileName = entity.FileName,
                OriginalFileName = entity.OriginalFileName,
                FileSizeMb = entity.FileSizeMb,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                CreatedByEmployeeId = entity.CreatedByEmployeeId,
                ModifiedByEmployeeId = entity.ModifiedByEmployeeId,
                IsActive = entity.IsActive,
                CreatedByEmployee = entity.CreatedByEmployee != null ? _employeesService.MapSingleEntityToDto(entity.CreatedByEmployee) : null,
                Customer = entity.Customer != null ? _customersService.MapSingleEntityToDto(entity.Customer) : null,
                DocumentCategory = entity.DocumentCategory != null ? _documentCategoriesService.MapSingleEntityToDto(entity.DocumentCategory) : null,
                DocumentType = entity.DocumentType != null ? _documentTypesService.MapSingleEntityToDto(entity.DocumentType) : null,
                Employee = entity.Employee != null ? _employeesService.MapSingleEntityToDto(entity.Employee) : null,
                ModifiedByEmployee = entity.ModifiedByEmployee != null ? _employeesService.MapSingleEntityToDto(entity.ModifiedByEmployee) : null,
                Rental = entity.Rental != null ? _rentalsService.MapSingleEntityToDto(entity.Rental) : null,
                RentalPlace = entity.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(entity.RentalPlace) : null,
                Vehicle = entity.Vehicle != null ? _vehiclesService.MapSingleEntityToDto(entity.Vehicle) : null
            };
        }

        protected override void UpdateEntity(Document entity, DocumentDto dto)
        {
            entity.FileName = dto.FileName;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.ModifiedDate = DateTime.UtcNow;
            // entity.FileContent = dto.FileContent;
            entity.ModifiedByEmployeeId = dto.ModifiedByEmployeeId;

            if (dto.DocumentType != null)
            {
                entity.DocumentTypeId = dto.DocumentType.DocumentTypeId;
            }

            if (dto.DocumentCategory != null)
            {
                entity.DocumentCategoryId = dto.DocumentCategory.DocumentCategoryId;
            }

            //entity.RentalId = dto.RentalId;
            //entity.VehicleId = dto.VehicleId;
            //entity.EmployeeId = dto.EmployeeId;
            //entity.CustomerId = dto.CustomerId;
            //entity.RentalPlaceId = dto.RentalPlaceId;

            //if (dto.Vehicle != null)
            //{
            //    entity.VehicleId = dto.Vehicle.VehicleId;
            //}

            //if (dto.Employee != null)
            //{
            //    entity.EmployeeId = dto.Employee.Id;
            //}

            //if (dto.Customer != null)
            //{
            //    entity.CustomerId = dto.Customer.Id;
            //}

            //if (dto.RentalPlace != null)
            //{
            //    entity.RentalPlaceId = dto.RentalPlace.RentalPlaceId;
            //}

            //if (dto.Rental != null)
            //{
            //    entity.RentalId = dto.Rental.RentalId;
            //}

            // Restore the entity
            if (dto.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        // Upload a file
        public async Task<DocumentDto> UploadFileAsync(FileUploadDto uploadDto)
        {
            if (uploadDto.FileContent == null || uploadDto.FileContent.Length == 0)
            {
                throw new ArgumentException("No file uploaded or the file is empty.");
            }

            // Create document entity
            var document = new Document
            {
                DocumentTypeId = uploadDto.DocumentTypeId,
                DocumentCategoryId = uploadDto.DocumentCategoryId,
                VehicleId = uploadDto.VehicleId,
                EmployeeId = uploadDto.EmployeeId,
                CustomerId = uploadDto.CustomerId,
                RentalPlaceId = uploadDto.RentalPlaceId,
                RentalId = uploadDto.RentalId,
                Title = uploadDto.Title,
                Description = uploadDto.Description,
                CreatedByEmployeeId = uploadDto.CreatedByEmployeeId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            // Get category name
            var category = await _documentCategoriesService.FindEntityById(uploadDto.DocumentCategoryId);
            var categoryName = !string.IsNullOrWhiteSpace(category.Name) ? category.Name : "";

            // Extract extension
            var extension = Path.GetExtension(uploadDto.FileName).TrimStart('.');

            // Process the file
            document.FileName = $"{categoryName}_{uploadDto.Title}.{extension}";
            document.OriginalFileName = $"{categoryName}_{uploadDto.Title}.{extension}";
            document.FileSizeMb = uploadDto.FileContent.Length / 1024.0 / 1024.0;
            document.FileContent = uploadDto.FileContent;

            // Check if file size is too large
            var type = await _documentTypesService.FindEntityById(uploadDto.DocumentTypeId);
            if (document.FileSizeMb > type.MaxFileSizeMb)
            {
                throw new ArgumentException($"File size is too large. Maximum file size is {type.MaxFileSizeMb} MB.");
            }

            _dbSet.Add(document);
            await _context.SaveChangesAsync();

            return MapSingleEntityToDto(document);
        }

        // Download a file
        public async Task<(byte[] Content, string FileName)> DownloadFileAsync(int documentId)
        {
            var document = await _dbSet.FindAsync(documentId);
            if (document == null || document.FileContent == null)
            {
                throw new FileNotFoundException("Document not found or has no content.");
            }

            return (document.FileContent, document.OriginalFileName);
        }

        // Delete a file
        public override async Task<bool> DeleteAsync(int id)
        {
            var document = await FindEntityById(id);
            if (document == null)
            {
                return false;
            }

            document.IsActive = false;
            document.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
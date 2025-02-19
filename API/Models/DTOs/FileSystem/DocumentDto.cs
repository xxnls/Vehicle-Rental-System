using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Other;
using API.Models.DTOs.Rentals;
using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;

namespace API.Models.DTOs.FileSystem
{
    public class DocumentDto
    {
        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public int DocumentCategoryId { get; set; }
        public int? VehicleId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int? RentalPlaceId { get; set; }
        public int? RentalId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string FileName { get; set; } = null!;
        public string OriginalFileName { get; set; } = null!;
        public double FileSizeMb { get; set; }
        public byte[]? FileContent { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public EmployeeDto CreatedByEmployee { get; set; } = null!;

        public CustomerDto? Customer { get; set; }

        public DocumentCategoryDto DocumentCategory { get; set; } = null!;

        public DocumentTypeDto DocumentType { get; set; } = null!;

        public EmployeeDto? Employee { get; set; }

        public EmployeeDto? ModifiedByEmployee { get; set; }

        public RentalDto? Rental { get; set; }

        public RentalPlaceDto? RentalPlace { get; set; }

        public VehicleDto? Vehicle { get; set; }
    }
}

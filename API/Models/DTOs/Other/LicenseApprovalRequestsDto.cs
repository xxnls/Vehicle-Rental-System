using API.Interfaces;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.FileSystem;

namespace API.Models.DTOs.Other
{
    public class LicenseApprovalRequestsDto : IBaseModel
    {
        public int LicenseApprovalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int? ApprovedByEmployeeId { get; set; }
        public int? DocumentFrontId { get; set; }
        public int? DocumentBackId { get; set; }
        public string? LicenseType { get; set; }
        public string? RequestStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public CustomerDto Customer { get; set; } = null!;
        public DocumentDto? DocumentFront { get; set; } = null!;
        public DocumentDto? DocumentBack { get; set; } = null!;
        public EmployeeDto? ApprovedByEmployee { get; set; } = null!;
    }
}

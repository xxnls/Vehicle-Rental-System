using API.Interfaces;
using API.Models.Customers;
using API.Models.Employees;
using API.Models.FileSystem;

namespace API.Models.Other
{
    public enum LicenseType
    {
        A,
        B,
        C
    }

    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public partial class LicenseApprovalRequests : IBaseModel
    {
        public int LicenseApprovalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int? ApprovedByEmployeeId { get; set; }
        public int DocumentId { get; set; }
        public string? LicenseType { get; set; }
        public string? RequestStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Document Document { get; set; } = null!;
        public virtual Employee? ApprovedByEmployee { get; set; } = null!;
    }
}

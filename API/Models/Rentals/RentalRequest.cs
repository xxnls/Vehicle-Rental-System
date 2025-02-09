using API.Interfaces;
using API.Models.Customers;
using API.Models.Employees;
using API.Models.Vehicles;

namespace API.Models.Rentals
{
    public enum RentalRequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }

    public partial class RentalRequest : IBaseModel
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string? RequestStatus { get; set; } // Store as string
        public string? PaymentStatus { get; set; } // Store as string
        public string? Notes { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Vehicle Vehicle { get; set; } = null!;

        public virtual Employee? ModifiedByEmployee { get; set; }
    }
}

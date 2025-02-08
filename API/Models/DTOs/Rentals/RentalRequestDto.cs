using API.Interfaces;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Vehicles;

namespace API.Models.DTOs.Rentals
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

    public class RentalRequestDto : IBaseModel
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string? RequestStatus { get; set; } // String property in DTO
        public string? PaymentStatus { get; set; } // String property in DTO
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Navigation properties
        public CustomerDto Customer { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}

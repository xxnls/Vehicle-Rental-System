using API.Models.DTOs.Customers;
using API.Models.DTOs.Vehicles;

namespace API.Models.DTOs.Rentals
{
    public enum RentalRequestStatus // Define the enum
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public class RentalRequestDto
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public int VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; }
        public DateTime RequestDate { get; set; }
        public RentalRequestStatus RequestStatus { get; set; }
    }
}

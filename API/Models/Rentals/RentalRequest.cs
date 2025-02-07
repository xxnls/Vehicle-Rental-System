using API.Models.Customers;
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

    public partial class RentalRequest
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public DateTime RequestDate { get; set; }
        public RentalRequestStatus RequestStatus { get; set; }
    }
}

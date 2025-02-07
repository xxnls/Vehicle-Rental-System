using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.Models.DTOs.Rentals
{
    public enum RentalRequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public class RentalRequestDto : BaseDtoModel
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }

        private CustomerDto _customer;
        public CustomerDto Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged();
                }
            }
        }

        private VehicleDto _vehicle;
        public VehicleDto Vehicle
        {
            get => _vehicle;
            set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _requestDate;
        public DateTime RequestDate
        {
            get => _requestDate;
            set
            {
                if (_requestDate != value)
                {
                    _requestDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private RentalRequestStatus _requestStatus;
        public RentalRequestStatus RequestStatus
        {
            get => _requestStatus;
            set
            {
                if (_requestStatus != value)
                {
                    _requestStatus = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

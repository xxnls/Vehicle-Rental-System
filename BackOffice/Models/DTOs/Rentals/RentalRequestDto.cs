using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
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

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Cancelled
    }

    public class RentalRequestDto : BaseDtoModel
    {
        public int RentalRequestId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public int? ModifiedByEmployeeId { get; set; }

        private DateTime _requestDate = DateTime.Now;
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

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _endDate = DateTime.Now.AddDays(7);
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            set
            {
                if (_totalCost != value)
                {
                    _totalCost = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _requestStatus;
        public string RequestStatus
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

        private string _paymentStatus;
        public string PaymentStatus
        {
            get => _paymentStatus;
            set
            {
                if (_paymentStatus != value)
                {
                    _paymentStatus = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private EmployeeDto? _modifiedByEmployee;
        public EmployeeDto? ModifiedByEmployee
        {
            get => _modifiedByEmployee;
            set
            {
                if (_modifiedByEmployee != value)
                {
                    _modifiedByEmployee = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

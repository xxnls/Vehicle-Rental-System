using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.Models.DTOs.Rentals
{
    public enum RentalStatus
    {
        AwaitingPickup,
        InProgress,
        Finished,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum DepositStatus
    {
        Pending,
        NotTaken,
        PartiallyRefunded,
        FullyRefunded,
        FullyTaken
    }

    public class RentalDto : BaseDtoModel
    {
        public int RentalId { get; set; }

        public int? PostRentalReportId { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public int StartedByEmployeeId { get; set; }

        public int? FinishedByEmployeeId { get; set; }

        private string? _rentalStatus;
        public string? RentalStatus
        {
            get => _rentalStatus;
            set
            {
                if (_rentalStatus != value)
                {
                    _rentalStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _paymentStatus;
        public string? PaymentStatus
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

        private string? _depositStatus;
        public string? DepositStatus
        {
            get => _depositStatus;
            set
            {
                if (_depositStatus != value)
                {
                    _depositStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _damageFeePaymentStatus;
        public string? DamageFeePaymentStatus
        {
            get => _damageFeePaymentStatus;
            set
            {
                if (_damageFeePaymentStatus != value)
                {
                    _damageFeePaymentStatus = value;
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

        private DateTime? _pickupDateTime;
        public DateTime? PickupDateTime
        {
            get => _pickupDateTime;
            set
            {
                if (_pickupDateTime != value)
                {
                    _pickupDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _finishDateTime;
        public DateTime? FinishDateTime
        {
            get => _finishDateTime;
            set
            {
                if (_finishDateTime != value)
                {
                    _finishDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _depositAmount;
        public decimal DepositAmount
        {
            get => _depositAmount;
            set
            {
                if (_depositAmount != value)
                {
                    _depositAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? _depositRefundAmount;
        public decimal? DepositRefundAmount
        {
            get => _depositRefundAmount;
            set
            {
                if (_depositRefundAmount != value)
                {
                    _depositRefundAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? _finalCost;
        public decimal? FinalCost
        {
            get => _finalCost;
            set
            {
                if (_finalCost != value)
                {
                    _finalCost = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal? _damageFee;
        public decimal? DamageFee
        {
            get => _damageFee;
            set
            {
                if (_damageFee != value)
                {
                    _damageFee = value;
                    OnPropertyChanged();
                }
            }
        }

        private CustomerDto _customer = null!;
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

        private EmployeeDto? _finishedByEmployee;
        public EmployeeDto? FinishedByEmployee
        {
            get => _finishedByEmployee;
            set
            {
                if (_finishedByEmployee != value)
                {
                    _finishedByEmployee = value;
                    OnPropertyChanged();
                }
            }
        }

        private PostRentalReportDto? _postRentalReport;
        public PostRentalReportDto? PostRentalReport
        {
            get => _postRentalReport;
            set
            {
                if (_postRentalReport != value)
                {
                    _postRentalReport = value;
                    OnPropertyChanged();
                }
            }
        }

        private EmployeeDto _startedByEmployee = null!;
        public EmployeeDto StartedByEmployee
        {
            get => _startedByEmployee;
            set
            {
                if (_startedByEmployee != value)
                {
                    _startedByEmployee = value;
                    OnPropertyChanged();
                }
            }
        }

        private VehicleDto _vehicle = null!;
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

        private decimal _depositDeduction = 0;
        public decimal DepositDeduction
        {
            get => _depositDeduction;
            set
            {
                if (_depositDeduction != value)
                {
                    _depositDeduction = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

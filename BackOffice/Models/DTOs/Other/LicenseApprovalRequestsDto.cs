using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.FileSystem;

namespace BackOffice.Models.DTOs.Other
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

    public class LicenseApprovalRequestsDto : BaseDtoModel
    {
        private int _customerId;
        private int? _approvedByEmployeeId;
        private int? _documentFrontId;
        private int? _documentBackId;
        private string? _licenseType;
        private string? _requestStatus;
        private CustomerDto _customer = null!;
        private DocumentDto? _documentFront = null!;
        private DocumentDto? _documentBack = null!;
        private EmployeeDto? _approvedByEmployee = null!;
        public int LicenseApprovalRequestId { get; set; }
        public int CustomerId
        {
            get => _customerId;
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    OnPropertyChanged(nameof(CustomerId));
                }
            }
        }

        public int? ApprovedByEmployeeId
        {
            get => _approvedByEmployeeId;
            set
            {
                if (_approvedByEmployeeId != value)
                {
                    _approvedByEmployeeId = value;
                    OnPropertyChanged(nameof(ApprovedByEmployeeId));
                }
            }
        }

        public int? DocumentFrontId
        {
            get => _documentFrontId;
            set
            {
                if (_documentFrontId != value)
                {
                    _documentFrontId = value;
                    OnPropertyChanged(nameof(DocumentFrontId));
                }
            }
        }

        public int? DocumentBackId
        {
            get => _documentBackId;
            set
            {
                if (_documentBackId != value)
                {
                    _documentBackId = value;
                    OnPropertyChanged(nameof(DocumentBackId));
                }
            }
        }

        public string? LicenseType
        {
            get => _licenseType;
            set
            {
                if (_licenseType != value)
                {
                    _licenseType = value;
                    OnPropertyChanged(nameof(LicenseType));
                }
            }
        }

        public string? RequestStatus
        {
            get => _requestStatus;
            set
            {
                if (_requestStatus != value)
                {
                    _requestStatus = value;
                    OnPropertyChanged(nameof(RequestStatus));
                }
            }
        }

        // Navigation properties
        public CustomerDto Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        public DocumentDto? DocumentFront
        {
            get => _documentFront;
            set
            {
                if (_documentFront != value)
                {
                    _documentFront = value;
                    OnPropertyChanged(nameof(DocumentFront));
                }
            }
        }

        public DocumentDto? DocumentBack
        {
            get => _documentBack;
            set
            {
                if (_documentBack != value)
                {
                    _documentBack = value;
                    OnPropertyChanged(nameof(DocumentBack));
                }
            }
        }

        public EmployeeDto? ApprovedByEmployee
        {
            get => _approvedByEmployee;
            set
            {
                if (_approvedByEmployee != value)
                {
                    _approvedByEmployee = value;
                    OnPropertyChanged(nameof(ApprovedByEmployee));
                }
            }
        }
    }
}

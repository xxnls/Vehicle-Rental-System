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
        private int _documentId;
        private string? _licenseType;
        private string? _requestStatus;
        private CustomerDto _customer = null!;
        private DocumentDto _document = null!;
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

        public int DocumentId
        {
            get => _documentId;
            set
            {
                if (_documentId != value)
                {
                    _documentId = value;
                    OnPropertyChanged(nameof(DocumentId));
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

        public DocumentDto Document
        {
            get => _document;
            set
            {
                if (_document != value)
                {
                    _document = value;
                    OnPropertyChanged(nameof(Document));
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

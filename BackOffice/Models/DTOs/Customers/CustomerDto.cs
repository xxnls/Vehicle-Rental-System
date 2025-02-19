using BackOffice.Interfaces;
using BackOffice.Models.Customers;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.Models.DTOs.Customers
{
    public class CustomerDto : BaseDtoModel
    {
        public int Id { get; set; }
        public int AddressId { get; set; }

        public int CustomerTypeId { get; set; }

        public int CustomerStatisticsId { get; set; }

        private string _firstName = null!;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName = null!;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string? _companyName;
        public string? CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        private string _email = null!;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber = null!;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private bool? _approvedA;
        public bool? ApprovedA
        {
            get => _approvedA;
            set
            {
                _approvedA = value;
                OnPropertyChanged();
            }
        }

        private bool? _approvedB;
        public bool? ApprovedB
        {
            get => _approvedB;
            set
            {
                _approvedB = value;
                OnPropertyChanged();
            }
        }

        private bool? _approvedC;
        public bool? ApprovedC
        {
            get => _approvedC;
            set
            {
                _approvedC = value;
                OnPropertyChanged();
            }
        }

        private string? _userName;
        public string? UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        // Navigation properties
        private AddressDto _address = new();
        public AddressDto Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private CustomerStatisticsDto _customerStatistics;
        public CustomerStatisticsDto CustomerStatistics
        {
            get => _customerStatistics;
            set
            {
                _customerStatistics = value;
                OnPropertyChanged();
            }
        }

        private CustomerTypeDto _customerType;
        public CustomerTypeDto CustomerType
        {
            get => _customerType;
            set
            {
                _customerType = value;
                OnPropertyChanged();
            }
        }

    }
}

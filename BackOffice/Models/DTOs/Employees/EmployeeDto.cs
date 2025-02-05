using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeDto : BaseDtoModel
    {
        // Backing fields
        private int _id;
        private int _employeeStatisticsId;
        private int _employeeFinancesId;
        private int _rentalPlaceId;
        private int _addressId;
        private int _employeePositionId;
        private int? _supervisorId;
        private string? _status;
        private string _firstName = null!;
        private string _lastName = null!;
        private DateTime? _dateOfBirth;
        private DateTime? _hireDate;
        private DateTime? _terminationDate;
        private string _email = null!;
        private string _phoneNumber = null!;
        private string _userName = null!;
        private string _password = null!;
        private AddressDto? _address = new();
        private EmployeeFinanceDto? _employeeFinances = new();
        private EmployeePositionDto? _employeePosition;
        private EmployeeStatisticsDto? _employeeStatistics;
        private RentalPlaceDto? _rentalPlace;
        private EmployeeSelectorDto? _supervisor;

        // Properties with explicit setters
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int EmployeeStatisticsId
        {
            get => _employeeStatisticsId;
            set
            {
                _employeeStatisticsId = value;
                OnPropertyChanged();
            }
        }

        public int EmployeeFinancesId
        {
            get => _employeeFinancesId;
            set
            {
                _employeeFinancesId = value;
                OnPropertyChanged();
            }
        }

        public int RentalPlaceId
        {
            get => _rentalPlaceId;
            set
            {
                _rentalPlaceId = value;
                OnPropertyChanged();
            }
        }

        public int AddressId
        {
            get => _addressId;
            set
            {
                _addressId = value;
                OnPropertyChanged();
            }
        }

        public int EmployeePositionId
        {
            get => _employeePositionId;
            set
            {
                _employeePositionId = value;
                OnPropertyChanged();
            }
        }

        public int? SupervisorId
        {
            get => _supervisorId;
            set
            {
                _supervisorId = value;
                OnPropertyChanged();
            }
        }

        public string? Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        public DateTime? HireDate
        {
            get => _hireDate;
            set
            {
                _hireDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? TerminationDate
        {
            get => _terminationDate;
            set
            {
                _terminationDate = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public AddressDto? Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public EmployeeFinanceDto? EmployeeFinances
        {
            get => _employeeFinances;
            set
            {
                _employeeFinances = value;
                OnPropertyChanged();
            }
        }

        public EmployeePositionDto? EmployeePosition
        {
            get => _employeePosition;
            set
            {
                _employeePosition = value;
                OnPropertyChanged();
            }
        }

        public EmployeeStatisticsDto? EmployeeStatistics
        {
            get => _employeeStatistics;
            set
            {
                _employeeStatistics = value;
                OnPropertyChanged();
            }
        }

        public RentalPlaceDto? RentalPlace
        {
            get => _rentalPlace;
            set
            {
                _rentalPlace = value;
                OnPropertyChanged();
            }
        }

        public EmployeeSelectorDto? Supervisor
        {
            get => _supervisor;
            set
            {
                _supervisor = value;
                OnPropertyChanged();
            }
        }
    }
}

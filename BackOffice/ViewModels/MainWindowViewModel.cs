using BackOffice.Helpers;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Properties;
using BackOffice.Views;
using BackOffice.ViewModels.Employees;
using BackOffice.ViewModels.Other;
using BackOffice.ViewModels.Vehicles;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Rentals;

namespace BackOffice.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties & Fields

        private bool _canManageVehicles;
        private bool _canManageEmployees;
        private bool _canManageRentals;
        private bool _canManageLeaves;
        private bool _canManageSchedule;
        private bool _isUserAdmin;

        public bool CanManageVehicles
        {
            get => _canManageVehicles;
            set
            {
                if (_canManageVehicles != value)
                {
                    _canManageVehicles = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanManageEmployees
        {
            get => _canManageEmployees;
            set
            {
                if (_canManageEmployees != value)
                {
                    _canManageEmployees = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanManageRentals
        {
            get => _canManageRentals;
            set
            {
                if (_canManageRentals != value)
                {
                    _canManageRentals = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanManageLeaves
        {
            get => _canManageLeaves;
            set
            {
                if (_canManageLeaves != value)
                {
                    _canManageLeaves = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanManageSchedule
        {
            get => _canManageSchedule;
            set
            {
                if (_canManageSchedule != value)
                {
                    _canManageSchedule = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsUserAdmin
        {
            get => _isUserAdmin;
            set
            {
                if (_isUserAdmin != value)
                {
                    _isUserAdmin = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly Dictionary<string, object> _viewModelMappings;

        public string FullName => CurrentUser.FirstName + " " + CurrentUser.LastName;

        private string _userRoles;
        public string UserRoles
        {
            get => _userRoles;
            set
            {
                _userRoles = value;
                OnPropertyChanged();
            }
        }

        private double _sidebarWidth = 200;
        public double SidebarWidth
        {
            get => _sidebarWidth;
            set
            {
                if (_sidebarWidth != value)
                {
                    _sidebarWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _currentWorkspace;
        public object CurrentWorkspace
        {
            get => _currentWorkspace;
            set
            {
                _currentWorkspace = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private EmployeeDto _currentUser;
        public EmployeeDto CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand ToggleSidebarCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ChangeWorkspaceCommand { get; }
        public ICommand ChangeLanguageCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            InitializePermissionsAsync();

            // Register to receive status messages
            WeakReferenceMessenger.Default.Register<Messenger>(this, (r, m) =>
            {
                StatusMessage = m.Value;
            });

            // Initialize mappings between string and view models
            _viewModelMappings = new Dictionary<string, object>
            {
                { "VehicleBrandsViewModel", new VehicleBrandsViewModel() },
                { "VehicleModelsViewModel", new VehicleModelsViewModel() },
                { "VehicleTypesViewModel", new VehicleTypesViewModel() },
                { "VehiclesViewModel", new VehiclesViewModel() },

                { "RentalPlacesViewModel", new RentalPlacesViewModel() },
                { "AddressesViewModel", new AddressesViewModel() },
                { "CountriesViewModel", new CountriesViewModel() },

                { "Employees", new EmployeesViewModel() },
                { "EmployeeShiftTypesViewModel", new EmployeeShiftTypesViewModel() },
                { "EmployeeLeaveTypesViewModel", new EmployeeLeaveTypesViewModel() },
                { "EmployeePositionsViewModel", new EmployeePositionsViewModel() },
                { "EmployeeSchedulesViewModel", new EmployeeSchedulesViewModel() },
                { "EmployeeRolesViewModel", new EmployeeRolesViewModel() },
                { "RolesAssignmentViewModel", new RolesAssignmentViewModel() },

                { "CustomerTypesViewModel", new CustomerTypesViewModel() },
                { "CustomersViewModel", new CustomersViewModel()},

                { "RentalRequestsViewModel", new RentalRequestsViewModel() },
                { "RentalApprovalsViewModel", new RentalApprovalsViewModel() },
                { "RentalsViewModel", new RentalsViewModel() },

                { "PaymentsViewModel", new PaymentsViewModel() }
            };

            // Load user
            CurrentUser = (EmployeeDto)SessionManager.Get("User");

            // Set default workspace
            CurrentWorkspace = _viewModelMappings["PaymentsViewModel"];

            ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
            LogoutCommand = new RelayCommand(Logout);
            ChangeWorkspaceCommand = new RelayCommand<object>(ChangeWorkspace);
            ChangeLanguageCommand = new RelayCommand<string>(LocalizationHelper.SetLanguage);

            // Get user roles form SessionManager and set them into the UserRoles property
            if (SessionManager.Get("Roles") is List<string> roles)
            {
                UserRoles = string.Join(", ", roles);
            }
        }

        #region Methods

        /// <summary>
        /// Initialize permissions for the current user
        /// </summary>
        private async void InitializePermissionsAsync()
        {
            CanManageVehicles = await RoleHelper.HasPermission("ManageVehicles");
            CanManageEmployees = await RoleHelper.HasPermission("ManageEmployees");
            CanManageRentals = await RoleHelper.HasPermission("ManageRentals");
            CanManageLeaves = await RoleHelper.HasPermission("ManageLeaves");
            CanManageSchedule = await RoleHelper.HasPermission("ManageSchedule");
            IsUserAdmin = RoleHelper.HasRole("Admin");
        }

        private void ToggleSidebar()
        {
            SidebarWidth = SidebarWidth == 200 ? 52 : 200;
        }

        private void ChangeWorkspace(object parameter)
        {
            if (parameter is string viewModelKey && _viewModelMappings.ContainsKey(viewModelKey))
            {
                CurrentWorkspace = _viewModelMappings[viewModelKey];
            }
        }

        /// <summary>
        /// Send a message to log out the user
        /// </summary>
        private void Logout()
        {
            WeakReferenceMessenger.Default.Send(new Messenger("LogoutSuccessful"));
        }

        #endregion
    }
}

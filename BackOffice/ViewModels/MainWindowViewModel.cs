﻿using BackOffice.Helpers;
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

namespace BackOffice.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Properties & Fields
        private readonly Dictionary<string, object> _viewModelMappings;

        public string FullName { get { return CurrentUser.FirstName + " " + CurrentUser.LastName; } }

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
                { "EmployeePositionsViewModel", new EmployeePositionsViewModel() }
            };

            // Load user
            CurrentUser = (EmployeeDto)SessionManager.Get("User");

            // Set default workspace
            CurrentWorkspace = _viewModelMappings["Employees"];

            ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
            LogoutCommand = new RelayCommand(Logout);
            ChangeWorkspaceCommand = new RelayCommand<object>(ChangeWorkspace);
            ChangeLanguageCommand = new RelayCommand<string>(LocalizationHelper.SetLanguage);
        }

        #region Methods
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

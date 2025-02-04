using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using BackOffice.Views.Employees;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.ViewModels.Employees
{
    public class EmployeesViewModel : BaseListViewModel<EmployeeDto>, IListViewModel
    {
        private bool _isSalaryDisabled;
        private bool _isHourlyRateDisabled;

        public bool IsSalaryDisabled
        {
            get => _isSalaryDisabled;
            set
            {
                if (_isSalaryDisabled != value)
                {
                    _isSalaryDisabled = value;
                    OnPropertyChanged();
                    if (_isSalaryDisabled)
                    {
                        EditableModel.EmployeeFinances.BaseSalary = null;
                    }
                }
            }
        }

        public bool IsHourlyRateDisabled
        {
            get => _isHourlyRateDisabled;
            set
            {
                if (_isHourlyRateDisabled != value)
                {
                    _isHourlyRateDisabled = value;
                    OnPropertyChanged();
                    if (_isHourlyRateDisabled)
                    {
                        EditableModel.EmployeeFinances.HourlyRate = null;
                    }
                }
            }
        }
        public SelectorDialogParameters SelectEmployeePositionParameters { get; set; }
        public SelectorDialogParameters SelectRentalPlaceParameters { get; set; }
        public SelectorDialogParameters SelectSupervisorParameters { get; set; }
        public SelectorDialogParameters SelectCountryParameters { get; set; }

        public EmployeesViewModel() : base("Employees", LocalizationHelper.GetString("Employees", "DisplayName"))
        {
            SelectEmployeePositionParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(EmployeePositionsViewModel),
                SelectorView = new EmployeePositionsView(),
                TargetProperty = result =>
                {
                    EditableModel.EmployeePosition ??= new EmployeePositionDto();
                    EditableModel.EmployeePosition = (EmployeePositionDto)result;
                },
                Title = LocalizationHelper.GetString("Employees", "SelectEmployeePositionTitle")
            };

            SelectRentalPlaceParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(RentalPlacesViewModel),
                SelectorView = new RentalPlacesView(),
                TargetProperty = result =>
                {
                    EditableModel.RentalPlace ??= new RentalPlaceDto();
                    EditableModel.RentalPlace = (RentalPlaceDto)result;
                },
                Title = LocalizationHelper.GetString("Employees", "SelectRentalPlaceTitle")
            };

            SelectSupervisorParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(EmployeeSelectorViewModel),
                SelectorView = new EmployeeSelectorView(),
                TargetProperty = result =>
                {
                    EditableModel.Supervisor ??= new EmployeeSelectorDto();
                    EditableModel.Supervisor = (EmployeeSelectorDto)result;
                },
                Title = LocalizationHelper.GetString("Employees", "SelectSupervisorTitle")
            };

            SelectCountryParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CountriesViewModel),
                SelectorView = new CountriesView(),
                TargetProperty = result =>
                {
                    EditableModel.Address.Country ??= new CountryDto();
                    EditableModel.Address.Country = (CountryDto)result;
                },
                Title = LocalizationHelper.GetString("Employees", "SelectCountryTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.Id, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.Id),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.FirstName), ValidateFirstName },
                { nameof(EditableModel.LastName), ValidateLastName },
                { nameof(EditableModel.DateOfBirth), ValidateDateOfBirth },
                { nameof(EditableModel.HireDate), ValidateHireDate },
                { nameof(EditableModel.TerminationDate), ValidateTerminationDate },
                { nameof(EditableModel.Email), ValidateEmail },
                { nameof(EditableModel.PhoneNumber), ValidatePhoneNumber },
                // { nameof(EditableModel.UserName), ValidateUserName },
                // { nameof(EditableModel.Address), ValidateAddress },
                { nameof(EditableModel.EmployeePosition), ValidateEmployeePosition },
                { nameof(EditableModel.RentalPlace), ValidateRentalPlace }, { nameof(EditableModel.Supervisor), ValidateSupervisor }
            };
        }

        #region Validation
        // Validation method for FirstName
        private void ValidateFirstName()
        {
            ClearErrors(nameof(EditableModel.FirstName));

            if (string.IsNullOrWhiteSpace(EditableModel.FirstName))
            {
                AddError(nameof(EditableModel.FirstName), LocalizationHelper.GetString("Employees", "ErrorFirstName1"));
            }
            else if (EditableModel.FirstName.Length > 100)
            {
                AddError(nameof(EditableModel.FirstName), LocalizationHelper.GetString("Employees", "ErrorFirstName2"));
            }
        }

        // Validation method for LastName
        private void ValidateLastName()
        {
            ClearErrors(nameof(EditableModel.LastName));

            if (string.IsNullOrWhiteSpace(EditableModel.LastName))
            {
                AddError(nameof(EditableModel.LastName), LocalizationHelper.GetString("Employees", "ErrorLastName1"));
            }
            else if (EditableModel.LastName.Length > 100)
            {
                AddError(nameof(EditableModel.LastName), LocalizationHelper.GetString("Employees", "ErrorLastName2"));
            }
        }

        // Validation method for DateOfBirth
        private void ValidateDateOfBirth()
        {
            ClearErrors(nameof(EditableModel.DateOfBirth));

            if (EditableModel.DateOfBirth == default)
            {
                AddError(nameof(EditableModel.DateOfBirth), LocalizationHelper.GetString("Employees", "ErrorDateOfBirth1"));
            }
            else if (EditableModel.DateOfBirth > DateTime.Now)
            {
                AddError(nameof(EditableModel.DateOfBirth), LocalizationHelper.GetString("Employees", "ErrorDateOfBirth2"));
            }
        }

        // Validation method for HireDate
        private void ValidateHireDate()
        {
            ClearErrors(nameof(EditableModel.HireDate));

            if (EditableModel.HireDate == default)
            {
                AddError(nameof(EditableModel.HireDate), LocalizationHelper.GetString("Employees", "ErrorHireDate1"));
            }
            else if (EditableModel.HireDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.HireDate), LocalizationHelper.GetString("Employees", "ErrorHireDate2"));
            }
        }

        // Validation method for TerminationDate
        private void ValidateTerminationDate()
        {
            ClearErrors(nameof(EditableModel.TerminationDate));

            if (EditableModel.TerminationDate.HasValue && EditableModel.TerminationDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.TerminationDate), LocalizationHelper.GetString("Employees", "ErrorTerminationDate1"));
            }
        }

        // Validation method for Email
        private void ValidateEmail()
        {
            ClearErrors(nameof(EditableModel.Email));

            if (string.IsNullOrWhiteSpace(EditableModel.Email))
            {
                AddError(nameof(EditableModel.Email), LocalizationHelper.GetString("Employees", "ErrorEmail1"));
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(EditableModel.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                AddError(nameof(EditableModel.Email), LocalizationHelper.GetString("Employees", "ErrorEmail2"));
            }
        }

        // Validation method for PhoneNumber
        private void ValidatePhoneNumber()
        {
            ClearErrors(nameof(EditableModel.PhoneNumber));

            if (string.IsNullOrWhiteSpace(EditableModel.PhoneNumber))
            {
                AddError(nameof(EditableModel.PhoneNumber), LocalizationHelper.GetString("Employees", "ErrorPhoneNumber1"));
            }
            else if (EditableModel.PhoneNumber.Length > 20)
            {
                AddError(nameof(EditableModel.PhoneNumber), LocalizationHelper.GetString("Employees", "ErrorPhoneNumber2"));
            }
        }

        // Validation method for UserName
        private void ValidateUserName()
        {
            ClearErrors(nameof(EditableModel.UserName));

            if (string.IsNullOrWhiteSpace(EditableModel.UserName))
            {
                AddError(nameof(EditableModel.UserName), LocalizationHelper.GetString("Employees", "ErrorUserName1"));
            }
            else if (EditableModel.UserName.Length > 50)
            {
                AddError(nameof(EditableModel.UserName), LocalizationHelper.GetString("Employees", "ErrorUserName2"));
            }
        }

        // Validation method for Address
        private void ValidateAddress()
        {
            ClearErrors(nameof(EditableModel.Address));

            if (EditableModel.Address == null)
            {
                AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Employees", "ErrorAddress1"));
            }
        }

        // Validation method for EmployeePosition
        private void ValidateEmployeePosition()
        {
            ClearErrors(nameof(EditableModel.EmployeePosition));

            if (EditableModel.EmployeePosition == null)
            {
                AddError(nameof(EditableModel.EmployeePosition), LocalizationHelper.GetString("Employees", "ErrorEmployeePosition1"));
            }
        }

        // Validation method for RentalPlace
        private void ValidateRentalPlace()
        {
            ClearErrors(nameof(EditableModel.RentalPlace));

            if (EditableModel.RentalPlace == null)
            {
                AddError(nameof(EditableModel.RentalPlace), LocalizationHelper.GetString("Employees", "ErrorRentalPlace1"));
            }
        }

        // Validation method for Supervisor
        private void ValidateSupervisor()
        {
            ClearErrors(nameof(EditableModel.Supervisor));

            if (EditableModel.Supervisor != null && EditableModel.Supervisor.Id == EditableModel.Id)
            {
                AddError(nameof(EditableModel.Supervisor), LocalizationHelper.GetString("Employees", "ErrorSupervisor1"));
            }
        }

        #endregion
    }
}
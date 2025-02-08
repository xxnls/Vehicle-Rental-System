using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Vehicles;
using BackOffice.Views.Customers;
using BackOffice.Views.Vehicles;

namespace BackOffice.ViewModels.Rentals
{
    public class RentalRequestsViewModel : BaseListViewModel<RentalRequestDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCustomerParameters { get; set; }
        public SelectorDialogParameters SelectVehicleParameters { get; set; }

        public RentalRequestsViewModel() : base("RentalRequests", LocalizationHelper.GetString("RentalRequests", "DisplayName"))
        {
            SelectCustomerParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CustomersViewModel),
                SelectorView = new CustomersView(),
                TargetProperty = result =>
                {
                    EditableModel.Customer ??= new CustomerDto();
                    EditableModel.Customer = (CustomerDto)result;
                },
                Title = LocalizationHelper.GetString("RentalRequests", "SelectCustomer")
            };

            SelectVehicleParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehiclesViewModel),
                SelectorView = new VehiclesView(),
                TargetProperty = result =>
                {
                    EditableModel.Vehicle ??= new VehicleDto();
                    EditableModel.Vehicle = (VehicleDto)result;
                },
                Title = LocalizationHelper.GetString("RentalRequests", "SelectVehicle")
            };

            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.RentalRequestId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.RentalRequestId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Customer), ValidateCustomer },
                { nameof(EditableModel.Vehicle), ValidateVehicle },
                { nameof(EditableModel.RequestDate), ValidateRequestDate },
                { nameof(EditableModel.StartDate),  ValidateStartDate },
                { nameof(EditableModel.EndDate), ValidateEndDate }
            };
        }

        #region Validation

        // Validation method for Customer
        private void ValidateCustomer()
        {
            ClearErrors(nameof(EditableModel.Customer));

            if (EditableModel.Customer == null)
            {
                AddError(nameof(EditableModel.Customer), LocalizationHelper.GetString("RentalRequests", "ErrorCustomer1"));
            }
        }

        // Validation method for Vehicle
        private void ValidateVehicle()
        {
            ClearErrors(nameof(EditableModel.Vehicle));

            if (EditableModel.Vehicle == null)
            {
                AddError(nameof(EditableModel.Vehicle), LocalizationHelper.GetString("RentalRequests", "ErrorVehicle1"));
            }
        }

        // Validation method for RequestDate
        private void ValidateRequestDate()
        {
            ClearErrors(nameof(EditableModel.RequestDate));

            if (EditableModel.RequestDate == default)
            {
                AddError(nameof(EditableModel.RequestDate), LocalizationHelper.GetString("RentalRequests", "ErrorRequestDate1"));
            }
            else if (EditableModel.RequestDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.RequestDate), LocalizationHelper.GetString("RentalRequests", "ErrorRequestDate2"));
            }
        }

        // Validation method for StartDate
        private void ValidateStartDate()
        {
            ClearErrors(nameof(EditableModel.StartDate));
            if (EditableModel.StartDate == default)
            {
                AddError(nameof(EditableModel.StartDate), LocalizationHelper.GetString("RentalRequests", "ErrorStartDate1"));
            }
            else if (EditableModel.StartDate < DateTime.Now.AddDays(-1))
            {
                AddError(nameof(EditableModel.StartDate), LocalizationHelper.GetString("RentalRequests", "ErrorStartDate2"));
            }
            else if (EditableModel.StartDate > EditableModel.EndDate)
            {
                AddError(nameof(EditableModel.StartDate), LocalizationHelper.GetString("RentalRequests", "ErrorStartDate3"));
            }
        }

        // Validation method for EndDate
        private void ValidateEndDate()
        {
            ClearErrors(nameof(EditableModel.EndDate));
            if (EditableModel.EndDate == default)
            {
                AddError(nameof(EditableModel.EndDate), LocalizationHelper.GetString("RentalRequests", "ErrorEndDate1"));
            }
            else if (EditableModel.EndDate < DateTime.Now)
            {
                AddError(nameof(EditableModel.EndDate), LocalizationHelper.GetString("RentalRequests", "ErrorEndDate2"));
            }
            else if (EditableModel.EndDate < EditableModel.StartDate)
            {
                AddError(nameof(EditableModel.EndDate), LocalizationHelper.GetString("RentalRequests", "ErrorEndDate3"));
            }
        }

        #endregion
    }
}
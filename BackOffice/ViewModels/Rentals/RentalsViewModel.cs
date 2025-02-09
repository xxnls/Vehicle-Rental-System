using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.DTOs.Rentals;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.ViewModels.Customers;
using BackOffice.ViewModels.Vehicles;
using BackOffice.Views.Customers;
using BackOffice.Views.Vehicles;

namespace BackOffice.ViewModels.Rentals
{
    public class RentalsViewModel : BaseListViewModel<RentalDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCustomerParameters { get; set; }
        public SelectorDialogParameters SelectVehicleParameters { get; set; }

        public RentalsViewModel() : base("Rentals", LocalizationHelper.GetString("Rentals", "DisplayName"))
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
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.RentalId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.RentalId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Customer), ValidateCustomer },
                { nameof(EditableModel.Vehicle), ValidateVehicle },
                { nameof(EditableModel.StartDate), ValidateStartDate },
                { nameof(EditableModel.EndDate), ValidateEndDate },
            };
        }

        private async Task CreateModelAsync(RentalDto rental)
        {
            if (SessionManager.Get("User") != null)
            {
                rental.StartedByEmployee = (EmployeeDto?)SessionManager.Get("User");
            }
            else
            {
                return;
            }

            await base.CreateModelAsync(rental);
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

        // Validation method for StartDate
        private void ValidateStartDate()
        {
            ClearErrors(nameof(EditableModel.StartDate));
            if (EditableModel.StartDate == default)
            {
                AddError(nameof(EditableModel.StartDate), LocalizationHelper.GetString("RentalRequests", "ErrorStartDate1"));
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
            else if (EditableModel.EndDate < EditableModel.StartDate)
            {
                AddError(nameof(EditableModel.EndDate), LocalizationHelper.GetString("RentalRequests", "ErrorEndDate3"));
            }
        }

        #endregion
    }
}

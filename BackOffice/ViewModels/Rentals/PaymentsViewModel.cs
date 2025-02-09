using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Customers;
using BackOffice.Views.Customers;
using BackOffice.Views.Rentals;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Rentals;

namespace BackOffice.ViewModels.Rentals
{
    public class PaymentsViewModel : BaseListViewModel<PaymentDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCustomerParameters { get; set; }
        public SelectorDialogParameters SelectRentalParameters { get; set; }

        public PaymentsViewModel() : base("Payments", LocalizationHelper.GetString("Payments", "DisplayName"))
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
                Title = LocalizationHelper.GetString("Payments", "SelectCustomer") 
            };

            SelectRentalParameters = new SelectorDialogParameters 
            {
                SelectorViewModelType = typeof(RentalsViewModel),
                SelectorView = new RentalsView(),
                TargetProperty = result =>
                {
                    EditableModel.Rent ??= new RentalDto();
                    EditableModel.Rent = (RentalDto)result;
                },
                Title = LocalizationHelper.GetString("Payments", "SelectRental")
            };


            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.PaymentId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.PaymentId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Customer), ValidateCustomer },
                { nameof(EditableModel.Rent), ValidateRent },
                { nameof(EditableModel.Amount), ValidateAmount },
                { nameof(EditableModel.PaymentDate), ValidatePaymentDate },
            };
        }

        #region Validation

        private void ValidateCustomer()
        {
            ClearErrors(nameof(EditableModel.Customer));
            if (EditableModel.Customer == null)
            {
                AddError(nameof(EditableModel.Customer), LocalizationHelper.GetString("Payments", "ErrorCustomer1"));
            }
        }

        private void ValidateRent()
        {
            ClearErrors(nameof(EditableModel.Rent));
            if (EditableModel.Rent == null)
            {
                AddError(nameof(EditableModel.Rent), LocalizationHelper.GetString("Payments", "ErrorRent1"));
            }
        }

        private void ValidateAmount()
        {
            ClearErrors(nameof(EditableModel.Amount));
            if (EditableModel.Amount <= 0)
            {
                AddError(nameof(EditableModel.Amount), LocalizationHelper.GetString("Payments", "ErrorAmount1"));
            }
        }

        private void ValidatePaymentDate()
        {
            ClearErrors(nameof(EditableModel.PaymentDate));
            if (EditableModel.PaymentDate == default)
            {
                AddError(nameof(EditableModel.PaymentDate), LocalizationHelper.GetString("Payments", "ErrorPaymentDate1"));
            }
            else if (EditableModel.PaymentDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.PaymentDate), LocalizationHelper.GetString("Payments", "ErrorPaymentDate2"));
            }
        }

        #endregion
    }
}

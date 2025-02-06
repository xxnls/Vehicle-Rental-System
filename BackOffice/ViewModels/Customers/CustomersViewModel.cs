using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using BackOffice.Views.Customers;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using BackOffice.Models.DTOs.Customers;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.ViewModels.Customers
{
    public class CustomersViewModel : BaseListViewModel<CustomerDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCustomerTypeParameters { get; set; }
        public SelectorDialogParameters SelectCountryParameters { get; set; }

        public CustomersViewModel() : base("Customers", LocalizationHelper.GetString("Customers", "DisplayName"))
        {
            SelectCustomerTypeParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CustomerTypesViewModel),
                SelectorView = new CustomerTypesView(),
                TargetProperty = result =>
                {
                    EditableModel.CustomerType ??= new CustomerTypeDto();
                    EditableModel.CustomerType = (CustomerTypeDto)result;
                },
                Title = LocalizationHelper.GetString("Customers", "SelectCustomerTypeTitle")
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
                Title = LocalizationHelper.GetString("Customers", "SelectCountryTitle")
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
                { nameof(EditableModel.Email), ValidateEmail },
                { nameof(EditableModel.PhoneNumber), ValidatePhoneNumber },
                // { nameof(EditableModel.Address), ValidateAddress },
                { nameof(EditableModel.CustomerType), ValidateCustomerType }
            };
        }

        #region Validation
        // Validation method for FirstName
        private void ValidateFirstName()
        {
            ClearErrors(nameof(EditableModel.FirstName));

            if (string.IsNullOrWhiteSpace(EditableModel.FirstName))
            {
                AddError(nameof(EditableModel.FirstName), LocalizationHelper.GetString("Customers", "ErrorFirstName1"));
            }
            else if (EditableModel.FirstName.Length > 50)
            {
                AddError(nameof(EditableModel.FirstName), LocalizationHelper.GetString("Customers", "ErrorFirstName2"));
            }
        }

        // Validation method for LastName
        private void ValidateLastName()
        {
            ClearErrors(nameof(EditableModel.LastName));

            if (string.IsNullOrWhiteSpace(EditableModel.LastName))
            {
                AddError(nameof(EditableModel.LastName), LocalizationHelper.GetString("Customers", "ErrorLastName1"));
            }
            else if (EditableModel.LastName.Length > 50)
            {
                AddError(nameof(EditableModel.LastName), LocalizationHelper.GetString("Customers", "ErrorLastName2"));
            }
        }

        // Validation method for Email
        private void ValidateEmail()
        {
            ClearErrors(nameof(EditableModel.Email));

            if (string.IsNullOrWhiteSpace(EditableModel.Email))
            {
                AddError(nameof(EditableModel.Email), LocalizationHelper.GetString("Customers", "ErrorEmail1"));
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(EditableModel.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                AddError(nameof(EditableModel.Email), LocalizationHelper.GetString("Customers", "ErrorEmail2"));
            }
        }

        // Validation method for PhoneNumber
        private void ValidatePhoneNumber()
        {
            ClearErrors(nameof(EditableModel.PhoneNumber));

            if (string.IsNullOrWhiteSpace(EditableModel.PhoneNumber))
            {
                AddError(nameof(EditableModel.PhoneNumber), LocalizationHelper.GetString("Customers", "ErrorPhoneNumber1"));
            }
            else if (EditableModel.PhoneNumber.Length > 20)
            {
                AddError(nameof(EditableModel.PhoneNumber), LocalizationHelper.GetString("Customers", "ErrorPhoneNumber2"));
            }
        }

        //// Validation method for Address
        //private void ValidateAddress()
        //{
        //    ClearErrors(nameof(EditableModel.Address));

        //    if (EditableModel.Address == null)
        //    {
        //        AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Customers", "ErrorAddress1"));
        //    }
        //    else if (string.IsNullOrWhiteSpace(EditableModel.Address.FirstLine))
        //    {
        //        AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Customers", "ErrorAddress2"));
        //    }
        //    else if (string.IsNullOrWhiteSpace(EditableModel.Address.City))
        //    {
        //        AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Customers", "ErrorAddress3"));
        //    }
        //    else if (string.IsNullOrWhiteSpace(EditableModel.Address.ZipCode))
        //    {
        //        AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Customers", "ErrorAddress4"));
        //    }
        //    else if (EditableModel.Address.Country == null)
        //    {
        //        AddError(nameof(EditableModel.Address), LocalizationHelper.GetString("Customers", "ErrorAddress5"));
        //    }
        //}

        // Validation method for CustomerType
        private void ValidateCustomerType()
        {
            ClearErrors(nameof(EditableModel.CustomerType));

            if (EditableModel.CustomerType == null)
            {
                AddError(nameof(EditableModel.CustomerType), LocalizationHelper.GetString("Customers", "ErrorCustomerType1"));
            }
        }

        #endregion
    }
}
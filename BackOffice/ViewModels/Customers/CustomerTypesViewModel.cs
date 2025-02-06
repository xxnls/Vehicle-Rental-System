using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Customers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace BackOffice.ViewModels.Customers
{
    public class CustomerTypesViewModel : BaseListViewModel<CustomerTypeDto>, IListViewModel
    {
        public CustomerTypesViewModel() : base("CustomerTypes", LocalizationHelper.GetString("CustomerTypes", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.CustomerTypeId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.CustomerTypeId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                //{ nameof(EditableModel.CustomerType), ValidateCustomerType },
                { nameof(EditableModel.MaxRentals), ValidateMaxRentals },
                { nameof(EditableModel.DiscountPercent), ValidateDiscountPercent }
            };
        }

        #region Validation
        //private void ValidateCustomerType()
        //{
        //    ClearErrors(nameof(EditableModel.CustomerType));

        //    if (string.IsNullOrWhiteSpace(EditableModel.CustomerType))
        //    {
        //        AddError(nameof(EditableModel.CustomerType), LocalizationHelper.GetString("CustomerTypes", "ErrorCustomerType1"));
        //    }
        //    else if (EditableModel.CustomerType.Length < 3)
        //    {
        //        AddError(nameof(EditableModel.CustomerType), LocalizationHelper.GetString("CustomerTypes", "ErrorCustomerType2"));
        //    }
        //    else if (EditableModel.CustomerType.Length > 20)
        //    {
        //        AddError(nameof(EditableModel.CustomerType), LocalizationHelper.GetString("CustomerTypes", "ErrorCustomerType3"));
        //    }
        //}

        private void ValidateMaxRentals()
        {
            ValidateProperty(nameof(EditableModel.MaxRentals),
                () => EditableModel.MaxRentals == null || EditableModel.MaxRentals >= 0,
                LocalizationHelper.GetString("CustomerTypes", "ErrorMaxRentals1"));
        }

        private void ValidateDiscountPercent()
        {
            ValidateProperty(nameof(EditableModel.DiscountPercent),
                () => EditableModel.DiscountPercent == null || (EditableModel.DiscountPercent >= 0 && EditableModel.DiscountPercent <= 100),
                LocalizationHelper.GetString("CustomerTypes", "ErrorDiscountPercent1"));
        }
        #endregion
    }
}
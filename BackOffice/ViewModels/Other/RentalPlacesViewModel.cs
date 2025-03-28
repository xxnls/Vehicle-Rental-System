﻿using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Other;
using BackOffice.Views.Vehicles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Views.Other;

namespace BackOffice.ViewModels.Other
{
    public class RentalPlacesViewModel : BaseListViewModel<RentalPlaceDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCountryParameters { get; set; }

        public RentalPlacesViewModel() : base("RentalPlaces", LocalizationHelper.GetString("RentalPlaces", "DisplayName"))
        {
            SelectCountryParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CountriesViewModel),
                SelectorView = new CountriesView(),
                TargetProperty = result =>
                {
                    EditableModel.Address ??= new AddressDto();
                    EditableModel.Address.Country = (CountryDto)result;
                },
                Title = LocalizationHelper.GetString("RentalPlaces", "SelectCountryTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.RentalPlaceId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.RentalPlaceId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>();
        } 

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new RentalPlaceDto
            {
                Address = new AddressDto
                {
                    City = EditableModel.Address.City,
                    FirstLine = EditableModel.Address.FirstLine,
                    SecondLine = EditableModel.Address.SecondLine,
                    ZipCode = EditableModel.Address.ZipCode,
                    Country = EditableModel.Address.Country

                },

                Location = new LocationDto
                {
                    GpsLatitude = EditableModel.Location.GpsLatitude,
                    GpsLongitude = EditableModel.Location.GpsLongitude
                }
            };

            await CreateModelAsync(model);
        }

        #endregion

        #region Validation

        private void ValidateZipCode()
        {
            ClearErrors(nameof(EditableModel.Address.ZipCode));

            if (string.IsNullOrWhiteSpace(EditableModel.Address.ZipCode))
            {
                AddError(nameof(EditableModel.Address.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode1"));
            }
            else if (EditableModel.Address.ZipCode.Length > 20)
            {
                AddError(nameof(EditableModel.Address.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode2"));
            }
        }

        #endregion
    }
}

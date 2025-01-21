using BackOffice.Helpers;
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
        public SelectorDialogParameters SelectCountryIdParameters { get; set; }

        public RentalPlacesViewModel() : base("RentalPlaces", LocalizationHelper.GetString("RentalPlaces", "DisplayName"))
        {
            SelectCountryIdParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CountriesViewModel),
                SelectorView = new CountriesView(),
                TargetProperty = result => EditableModel.CountryId = (short)result,
                PropertyForSelection = "CountryId",
                Title = LocalizationHelper.GetString("RentalPlaces", "SelectCountryTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.RentalPlaceId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
            //    { nameof(EditableModel.LocationId), ValidateLocationId },
            //    { nameof(EditableModel.GpsLatitude), ValidateGpsLatitude },
            //    { nameof(EditableModel.GpsLongitude), ValidateGpsLongitude },
            //    { nameof(EditableModel.AddressId), ValidateAddressId },
            //    { nameof(EditableModel.City), ValidateCity },
            //    { nameof(EditableModel.FirstLine), ValidateFirstLine },
            //    { nameof(EditableModel.SecondLine), ValidateSecondLine }
            };
        } 

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new RentalPlaceDto
            {
                CountryId = EditableModel.CountryId,
                ZipCode = EditableModel.ZipCode,
                GpsLatitude = EditableModel.GpsLatitude,
                GpsLongitude = EditableModel.GpsLongitude,
                City = EditableModel.City,
                FirstLine = EditableModel.FirstLine,
                SecondLine = EditableModel.SecondLine
            };

            await CreateModelAsync(model);
        }

        public async Task UpdateModelAsync()
        {
            var id = EditableModel.RentalPlaceId;

            var model = new RentalPlaceDto
            {
                RentalPlaceId = EditableModel.RentalPlaceId,
                LocationId = EditableModel.LocationId,
                AddressId = EditableModel.AddressId,
                CountryId = EditableModel.CountryId,
                ZipCode = EditableModel.ZipCode,
                GpsLatitude = EditableModel.GpsLatitude,
                GpsLongitude = EditableModel.GpsLongitude,
                City = EditableModel.City,
                FirstLine = EditableModel.FirstLine,
                SecondLine = EditableModel.SecondLine
            };

            await UpdateModelAsync(id, model);
        }


        #endregion

        #region Validation


        #endregion
    }
}

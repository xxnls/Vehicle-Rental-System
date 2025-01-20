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

namespace BackOffice.ViewModels.Other
{
    public class RentalPlacesViewModel : BaseListViewModel<RentalPlaceDto>, IListViewModel
    {
        public RentalPlacesViewModel() : base("RentalPlaces", LocalizationHelper.GetString("RentalPlaces", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.RentalPlaceId),
                () => EditableModel != null
            );

            //ValidationRules = new Dictionary<string, Action>
            //{
            //    { nameof(EditableModel.LocationId), ValidateLocationId },
            //    { nameof(EditableModel.GpsLatitude), ValidateGpsLatitude },
            //    { nameof(EditableModel.GpsLongitude), ValidateGpsLongitude },
            //    { nameof(EditableModel.AddressId), ValidateAddressId },
            //    { nameof(EditableModel.City), ValidateCity },
            //    { nameof(EditableModel.FirstLine), ValidateFirstLine },
            //    { nameof(EditableModel.SecondLine), ValidateSecondLine }
            //};
        } 

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new RentalPlaceDto
            {
                LocationId = EditableModel.LocationId,
                AddressId = EditableModel.AddressId,
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
            };

            await UpdateModelAsync(id, model);
        }

        #endregion

        #region Validation


        #endregion
    }
}

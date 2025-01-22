using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.ViewModels.Other;
using BackOffice.Views.Other;
using BackOffice.Views.Vehicles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehiclesViewModel : BaseListViewModel<VehicleDto>, IListViewModel
    {
        public SelectorDialogParameters SelectVehicleModelIdParameters { get; set; }
        public SelectorDialogParameters SelectVehicleTypeIdParameters { get; set; }
        public SelectorDialogParameters SelectRentalPlaceIdParameters { get; set; }

        public VehiclesViewModel() : base("Vehicles", LocalizationHelper.GetString("Vehicles", "DisplayName"))
        {
            SelectVehicleModelIdParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleModelsViewModel),
                SelectorView = new VehicleModelsView(),
                TargetProperty = result => EditableModel.VehicleModelId = (int)result,
                PropertyForSelection = "VehicleModelId",
                Title = LocalizationHelper.GetString("Vehicles", "SelectVehicleModelTitle")
            };

            SelectVehicleTypeIdParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleTypesViewModel),
                SelectorView = new VehicleTypesView(),
                TargetProperty = result => EditableModel.VehicleTypeId = (int)result,
                PropertyForSelection = "VehicleTypeId",
                Title = LocalizationHelper.GetString("Vehicles", "SelectVehicleTypeTitle")
            };

            SelectRentalPlaceIdParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(RentalPlacesViewModel),
                SelectorView = new RentalPlacesView(),
                TargetProperty = result => EditableModel.RentalPlaceId = (int)result,
                PropertyForSelection = "RentalPlaceId",
                Title = LocalizationHelper.GetString("Vehicles", "SelectRentalPlaceTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.VehicleId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.VIN), ValidateVIN },
                { nameof(EditableModel.LicensePlate), ValidateLicensePlate },
                { nameof(EditableModel.Color), ValidateColor },
                { nameof(EditableModel.ManufactureYear), ValidateManufactureYear },
                { nameof(EditableModel.VehicleTypeId), ValidateVehicleTypeId },
                { nameof(EditableModel.VehicleModelId), ValidateVehicleModelId }
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new VehicleDto
            {
                VehicleTypeId = EditableModel.VehicleTypeId,
                VehicleModelId = EditableModel.VehicleModelId,
                RentalPlaceId = EditableModel.RentalPlaceId,
                VIN = EditableModel.VIN,
                LicensePlate = EditableModel.LicensePlate,
                Color = EditableModel.Color,
                ManufactureYear = EditableModel.ManufactureYear,
                CurrentMileage = EditableModel.CurrentMileage,
                LastMaintenanceMileage = EditableModel.LastMaintenanceMileage,
                LastMaintenanceDate = EditableModel.LastMaintenanceDate,
                NextMaintenanceDate = EditableModel.NextMaintenanceDate,
                PurchaseDate = EditableModel.PurchaseDate,
                PurchasePrice = EditableModel.PurchasePrice,
                Status = EditableModel.Status,
                CustomDailyRate = EditableModel.CustomDailyRate,
                CustomWeeklyRate = EditableModel.CustomWeeklyRate,
                CustomDeposit = EditableModel.CustomDeposit,
                IsAvailableForRent = EditableModel.IsAvailableForRent,
                Notes = EditableModel.Notes
            };

            await CreateModelAsync(model);
        }

        public async Task UpdateModelAsync()
        {
            var id = EditableModel.VehicleId;

            var model = new VehicleDto
            {
                VehicleId = EditableModel.VehicleId,
                VehicleTypeId = EditableModel.VehicleTypeId,
                VehicleModelId = EditableModel.VehicleModelId,
                RentalPlaceId = EditableModel.RentalPlaceId,
                VIN = EditableModel.VIN,
                LicensePlate = EditableModel.LicensePlate,
                Color = EditableModel.Color,
                ManufactureYear = EditableModel.ManufactureYear,
                CurrentMileage = EditableModel.CurrentMileage,
                LastMaintenanceMileage = EditableModel.LastMaintenanceMileage,
                LastMaintenanceDate = EditableModel.LastMaintenanceDate,
                NextMaintenanceDate = EditableModel.NextMaintenanceDate,
                PurchaseDate = EditableModel.PurchaseDate,
                PurchasePrice = EditableModel.PurchasePrice,
                Status = EditableModel.Status,
                CustomDailyRate = EditableModel.CustomDailyRate,
                CustomWeeklyRate = EditableModel.CustomWeeklyRate,
                CustomDeposit = EditableModel.CustomDeposit,
                IsAvailableForRent = EditableModel.IsAvailableForRent,
                Notes = EditableModel.Notes
            };

            await UpdateModelAsync(id, model);
        }

        #endregion

        #region Validation
        private void ValidateVIN()
        {
            ValidateProperty(nameof(EditableModel.VIN),
                () => !string.IsNullOrWhiteSpace(EditableModel.VIN) && EditableModel.VIN.Length == 17,
                LocalizationHelper.GetString("Vehicles", "ErrorVIN"));
        }

        private void ValidateLicensePlate()
        {
            ValidateProperty(nameof(EditableModel.LicensePlate),
                () => !string.IsNullOrWhiteSpace(EditableModel.LicensePlate),
                LocalizationHelper.GetString("Vehicles", "ErrorLicensePlate"));
        }

        private void ValidateColor()
        {
            ValidateProperty(nameof(EditableModel.Color),
                () => !string.IsNullOrWhiteSpace(EditableModel.Color),
                LocalizationHelper.GetString("Vehicles", "ErrorColor"));
        }

        private void ValidateManufactureYear()
        {
            ValidateProperty(nameof(EditableModel.ManufactureYear),
                () => EditableModel.ManufactureYear > 1900 && EditableModel.ManufactureYear <= DateTime.Now.Year,
                LocalizationHelper.GetString("Vehicles", "ErrorManufactureYear"));
        }

        private void ValidateVehicleTypeId()
        {
            ValidateProperty(nameof(EditableModel.VehicleTypeId),
                () => EditableModel.VehicleTypeId > 0,
                LocalizationHelper.GetString("Vehicles", "ErrorVehicleTypeId"));
        }

        private void ValidateVehicleModelId()
        {
            ValidateProperty(nameof(EditableModel.VehicleModelId),
                () => EditableModel.VehicleModelId > 0,
                LocalizationHelper.GetString("Vehicles", "ErrorVehicleModelId"));
        }

        #endregion
    }
}

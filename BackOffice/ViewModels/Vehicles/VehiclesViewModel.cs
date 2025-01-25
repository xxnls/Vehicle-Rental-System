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
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehiclesViewModel : BaseListViewModel<VehicleDto>, IListViewModel
    {
        public SelectorDialogParameters SelectVehicleModelParameters { get; set; }
        public SelectorDialogParameters SelectVehicleTypeParameters { get; set; }
        public SelectorDialogParameters SelectRentalPlaceParameters { get; set; }

        public VehiclesViewModel() : base("Vehicles", LocalizationHelper.GetString("Vehicles", "DisplayName"))
        {
            SelectVehicleModelParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleModelsViewModel),
                SelectorView = new VehicleModelsView(),
                TargetProperty = result =>
                {
                    EditableModel.VehicleModel ??= new VehicleModelDto();
                    EditableModel.VehicleModel = (VehicleModelDto)result;
                },
                Title = LocalizationHelper.GetString("Vehicles", "SelectVehicleModelTitle")
            };

            SelectVehicleTypeParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleTypesViewModel),
                SelectorView = new VehicleTypesView(),
                TargetProperty = result =>
                {
                    EditableModel.VehicleType ??= new VehicleTypeDto();
                    EditableModel.VehicleType = (VehicleTypeDto)result;
                },
                Title = LocalizationHelper.GetString("Vehicles", "SelectVehicleTypeTitle")
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
                Title = LocalizationHelper.GetString("Vehicles", "SelectRentalPlaceTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.VehicleId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.VehicleId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                //{ nameof(EditableModel.Vin), ValidateVIN },
                //{ nameof(EditableModel.LicensePlate), ValidateLicensePlate },
                //{ nameof(EditableModel.Color), ValidateColor },
                //{ nameof(EditableModel.ManufactureYear), ValidateManufactureYear },
                //{ nameof(EditableModel.VehicleTypeId), ValidateVehicleTypeId },
                //{ nameof(EditableModel.VehicleModelId), ValidateVehicleModelId }
            };
        }

        #region Methods

        //public async Task CreateModelAsync()
        //{
        //    var model = new VehicleDto
        //    {
        //        VehicleTypeId = EditableModel.VehicleTypeId,
        //        VehicleModelId = EditableModel.VehicleModelId,
        //        RentalPlaceId = EditableModel.RentalPlaceId,
        //        Vin = EditableModel.Vin,
        //        LicensePlate = EditableModel.LicensePlate,
        //        Color = EditableModel.Color,
        //        ManufactureYear = EditableModel.ManufactureYear,
        //        CurrentMileage = EditableModel.CurrentMileage,
        //        LastMaintenanceMileage = EditableModel.LastMaintenanceMileage,
        //        LastMaintenanceDate = EditableModel.LastMaintenanceDate,
        //        NextMaintenanceDate = EditableModel.NextMaintenanceDate,
        //        PurchaseDate = EditableModel.PurchaseDate,
        //        PurchasePrice = EditableModel.PurchasePrice,
        //        Status = EditableModel.Status,
        //        CustomDailyRate = EditableModel.CustomDailyRate,
        //        CustomWeeklyRate = EditableModel.CustomWeeklyRate,
        //        CustomDeposit = EditableModel.CustomDeposit,
        //        IsAvailableForRent = EditableModel.IsAvailableForRent,
        //        Notes = EditableModel.Notes
        //    };

        //    await CreateModelAsync(model);
        //}

        //public async Task UpdateModelAsync()
        //{
        //    var id = EditableModel.VehicleId;

        //    var model = new VehicleDto
        //    {
        //        VehicleId = EditableModel.VehicleId,
        //        VehicleTypeId = EditableModel.VehicleTypeId,
        //        VehicleModelId = EditableModel.VehicleModelId,
        //        RentalPlaceId = EditableModel.RentalPlaceId,
        //        Vin = EditableModel.Vin,
        //        LicensePlate = EditableModel.LicensePlate,
        //        Color = EditableModel.Color,
        //        ManufactureYear = EditableModel.ManufactureYear,
        //        CurrentMileage = EditableModel.CurrentMileage,
        //        LastMaintenanceMileage = EditableModel.LastMaintenanceMileage,
        //        LastMaintenanceDate = EditableModel.LastMaintenanceDate,
        //        NextMaintenanceDate = EditableModel.NextMaintenanceDate,
        //        PurchaseDate = EditableModel.PurchaseDate,
        //        PurchasePrice = EditableModel.PurchasePrice,
        //        Status = EditableModel.Status,
        //        CustomDailyRate = EditableModel.CustomDailyRate,
        //        CustomWeeklyRate = EditableModel.CustomWeeklyRate,
        //        CustomDeposit = EditableModel.CustomDeposit,
        //        IsAvailableForRent = EditableModel.IsAvailableForRent,
        //        Notes = EditableModel.Notes
        //    };

        //    await UpdateModelAsync(id, model);
        //}

        #endregion

        #region Validation
        private void ValidateVIN()
        {
            ValidateProperty(nameof(EditableModel.Vin),
                () => !string.IsNullOrWhiteSpace(EditableModel.Vin) && EditableModel.Vin.Length == 17,
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

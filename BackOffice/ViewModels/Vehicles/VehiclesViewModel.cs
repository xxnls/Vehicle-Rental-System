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
                { nameof(EditableModel.Vin), ValidateVin },
                { nameof(EditableModel.LicensePlate), ValidateLicensePlate },
                { nameof(EditableModel.Color), ValidateColor },
                { nameof(EditableModel.ManufactureYear), ValidateManufactureYear },
                { nameof(EditableModel.CurrentMileage), ValidateCurrentMileage },
                { nameof(EditableModel.LastMaintenanceMileage), ValidateLastMaintenanceMileage },
                { nameof(EditableModel.LastMaintenanceDate), ValidateLastMaintenanceDate },
                { nameof(EditableModel.NextMaintenanceDate), ValidateNextMaintenanceDate },
                { nameof(EditableModel.PurchaseDate), ValidatePurchaseDate },
                { nameof(EditableModel.PurchasePrice), ValidatePurchasePrice },
                { nameof(EditableModel.Status), ValidateStatus },
                { nameof(EditableModel.CustomDailyRate), ValidateCustomDailyRate },
                { nameof(EditableModel.CustomWeeklyRate), ValidateCustomWeeklyRate },
                { nameof(EditableModel.CustomDeposit), ValidateCustomDeposit },
                { nameof(EditableModel.Notes), ValidateNotes },
                { nameof(EditableModel.VehicleType), ValidateVehicleType },
                { nameof(EditableModel.VehicleModel), ValidateVehicleModel },
                { nameof(EditableModel.RentalPlace), ValidateRentalPlace }
            };
        }

        #region Validation
        // Validation method for Vin
        private void ValidateVin()
        {
            ClearErrors(nameof(EditableModel.Vin));

            if (string.IsNullOrWhiteSpace(EditableModel.Vin))
            {
                AddError(nameof(EditableModel.Vin), LocalizationHelper.GetString("Vehicles", "ErrorVin1"));
            }
            else if (EditableModel.Vin.Length != 17)
            {
                AddError(nameof(EditableModel.Vin), LocalizationHelper.GetString("Vehicles", "ErrorVin2"));
            }
        }

        // Validation method for LicensePlate
        private void ValidateLicensePlate()
        {
            ClearErrors(nameof(EditableModel.LicensePlate));

            if (string.IsNullOrWhiteSpace(EditableModel.LicensePlate))
            {
                AddError(nameof(EditableModel.LicensePlate), LocalizationHelper.GetString("Vehicles", "ErrorLicensePlate1"));
            }
            else if (EditableModel.LicensePlate.Length > 20)
            {
                AddError(nameof(EditableModel.LicensePlate), LocalizationHelper.GetString("Vehicles", "ErrorLicensePlate2"));
            }
        }

        // Validation method for Color
        private void ValidateColor()
        {
            ClearErrors(nameof(EditableModel.Color));

            if (string.IsNullOrWhiteSpace(EditableModel.Color))
            {
                AddError(nameof(EditableModel.Color), LocalizationHelper.GetString("Vehicles", "ErrorColor1"));
            }
        }

        // Validation method for ManufactureYear
        private void ValidateManufactureYear()
        {
            ClearErrors(nameof(EditableModel.ManufactureYear));

            if (string.IsNullOrWhiteSpace(EditableModel.ManufactureYear.ToString()))
            {
                AddError(nameof(EditableModel.ManufactureYear), LocalizationHelper.GetString("Vehicles", "ErrorManufactureYear2"));
            }
            else if (EditableModel.ManufactureYear < 1900 || EditableModel.ManufactureYear > DateTime.Now.Year)
            {
                AddError(nameof(EditableModel.ManufactureYear), LocalizationHelper.GetString("Vehicles", "ErrorManufactureYear1"));
            }
        }

        // Validation method for CurrentMileage
        private void ValidateCurrentMileage()
        {
            ClearErrors(nameof(EditableModel.CurrentMileage));

            if (string.IsNullOrWhiteSpace(EditableModel.CurrentMileage.ToString()))
            {
                AddError(nameof(EditableModel.CurrentMileage), LocalizationHelper.GetString("Vehicles", "ErrorCurrentMileage2"));
            }
            else if (EditableModel.CurrentMileage < 0)
            {
                AddError(nameof(EditableModel.CurrentMileage), LocalizationHelper.GetString("Vehicles", "ErrorCurrentMileage1"));
            }
        }

        // Validation method for LastMaintenanceMileage
        private void ValidateLastMaintenanceMileage()
        {
            ClearErrors(nameof(EditableModel.LastMaintenanceMileage));

            if (EditableModel.LastMaintenanceMileage.HasValue && EditableModel.LastMaintenanceMileage < 0)
            {
                AddError(nameof(EditableModel.LastMaintenanceMileage), LocalizationHelper.GetString("Vehicles", "ErrorLastMaintenanceMileage1"));
            }
        }

        // Validation method for LastMaintenanceDate
        private void ValidateLastMaintenanceDate()
        {
            ClearErrors(nameof(EditableModel.LastMaintenanceDate));

            if (EditableModel.LastMaintenanceDate.HasValue && EditableModel.LastMaintenanceDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.LastMaintenanceDate), LocalizationHelper.GetString("Vehicles", "ErrorLastMaintenanceDate1"));
            }
        }

        // Validation method for NextMaintenanceDate
        private void ValidateNextMaintenanceDate()
        {
            ClearErrors(nameof(EditableModel.NextMaintenanceDate));

            if (EditableModel.NextMaintenanceDate.HasValue && EditableModel.NextMaintenanceDate < DateTime.Now)
            {
                AddError(nameof(EditableModel.NextMaintenanceDate), LocalizationHelper.GetString("Vehicles", "ErrorNextMaintenanceDate1"));
            }
        }

        // Validation method for PurchaseDate
        private void ValidatePurchaseDate()
        {
            ClearErrors(nameof(EditableModel.PurchaseDate));

            if (EditableModel.PurchaseDate == null)
            {
                AddError(nameof(EditableModel.PurchaseDate), LocalizationHelper.GetString("Vehicles", "ErrorPurchaseDate2"));
            }
            else if (EditableModel.PurchaseDate > DateTime.Now)
            {
                AddError(nameof(EditableModel.PurchaseDate), LocalizationHelper.GetString("Vehicles", "ErrorPurchaseDate1"));
            }
        }

        // Validation method for PurchasePrice
        private void ValidatePurchasePrice()
        {
            ClearErrors(nameof(EditableModel.PurchasePrice));

            if (string.IsNullOrWhiteSpace(EditableModel.PurchasePrice.ToString()))
            {
                AddError(nameof(EditableModel.PurchasePrice), LocalizationHelper.GetString("Vehicles", "ErrorPurchasePrice2"));
            }
            else if (EditableModel.PurchasePrice <= 0)
            {
                AddError(nameof(EditableModel.PurchasePrice), LocalizationHelper.GetString("Vehicles", "ErrorPurchasePrice1"));
            }
        }

        // Validation method for Status
        private void ValidateStatus()
        {
            ClearErrors(nameof(EditableModel.Status));

            if (string.IsNullOrWhiteSpace(EditableModel.Status))
            {
                AddError(nameof(EditableModel.Status), LocalizationHelper.GetString("Vehicles", "ErrorStatus1"));
            }
        }

        // Validation method for CustomDailyRate
        private void ValidateCustomDailyRate()
        {
            ClearErrors(nameof(EditableModel.CustomDailyRate));

            if (EditableModel.CustomDailyRate.HasValue && EditableModel.CustomDailyRate <= 0)
            {
                AddError(nameof(EditableModel.CustomDailyRate), LocalizationHelper.GetString("Vehicles", "ErrorCustomDailyRate1"));
            }
        }

        // Validation method for CustomWeeklyRate
        private void ValidateCustomWeeklyRate()
        {
            ClearErrors(nameof(EditableModel.CustomWeeklyRate));

            if (EditableModel.CustomWeeklyRate.HasValue && EditableModel.CustomWeeklyRate <= 0)
            {
                AddError(nameof(EditableModel.CustomWeeklyRate), LocalizationHelper.GetString("Vehicles", "ErrorCustomWeeklyRate1"));
            }
        }

        // Validation method for CustomDeposit
        private void ValidateCustomDeposit()
        {
            ClearErrors(nameof(EditableModel.CustomDeposit));

            if (EditableModel.CustomDeposit.HasValue && EditableModel.CustomDeposit <= 0)
            {
                AddError(nameof(EditableModel.CustomDeposit), LocalizationHelper.GetString("Vehicles", "ErrorCustomDeposit1"));
            }
        }

        // Validation method for Notes
        private void ValidateNotes()
        {
            ClearErrors(nameof(EditableModel.Notes));

            if (!string.IsNullOrWhiteSpace(EditableModel.Notes) && EditableModel.Notes.Length > 500)
            {
                AddError(nameof(EditableModel.Notes), LocalizationHelper.GetString("Vehicles", "ErrorNotes1"));
            }
        }

        // Validation method for VehicleType
        private void ValidateVehicleType()
        {
            ClearErrors(nameof(EditableModel.VehicleType));

            if (EditableModel.VehicleType == null)
            {
                AddError(nameof(EditableModel.VehicleType), LocalizationHelper.GetString("Vehicles", "ErrorVehicleType1"));
            }
        }

        // Validation method for VehicleModel
        private void ValidateVehicleModel()
        {
            ClearErrors(nameof(EditableModel.VehicleModel));

            if (EditableModel.VehicleModel == null)
            {
                AddError(nameof(EditableModel.VehicleModel), LocalizationHelper.GetString("Vehicles", "ErrorVehicleModel1"));
            }
        }

        // Validation method for RentalPlace
        private void ValidateRentalPlace()
        {
            ClearErrors(nameof(EditableModel.RentalPlace));

            if (EditableModel.RentalPlace == null)
            {
                AddError(nameof(EditableModel.RentalPlace), LocalizationHelper.GetString("Vehicles", "ErrorRentalPlace1"));
            }
        }

        #endregion
    }
}

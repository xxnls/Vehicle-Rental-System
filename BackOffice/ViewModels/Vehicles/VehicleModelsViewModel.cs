using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Vehicles;
using BackOffice.Models.Vehicles;
using BackOffice.Models.Vehicles.VehicleBrands;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;
using BackOffice.Views;
using BackOffice.Views.CustomControls;
using BackOffice.Views.Vehicles;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehicleModelsViewModel : BaseListViewModel<VehicleModelDto>, IListViewModel
    {
        public SelectorDialogParameters SelectVehicleBrandParameters { get; set; }

        public VehicleModelsViewModel() : base("VehicleModels", LocalizationHelper.GetString("VehicleModels", "DisplayName"))
        {
            SelectVehicleBrandParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleBrandsViewModel),
                SelectorView = new VehicleBrandsView(),
                TargetProperty = result => EditableModel.VehicleBrand = (VehicleBrandDto)result,
                Title = LocalizationHelper.GetString("VehicleModels", "SelectVehicleBrandTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.VehicleModelId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.VehicleModelId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.Description), ValidateDescription},
                { nameof(EditableModel.EngineSize), ValidateEngineSize },
                { nameof(EditableModel.HorsePower), ValidateHorsePower },
                { nameof(EditableModel.FuelType), ValidateFuelType },
                { nameof(EditableModel.VehicleBrand), ValidateVehicleBrand }
            };
        }

        #region Validation
        // Validation method for Name
        private void ValidateName()
        {
            ClearErrors(nameof(EditableModel.Name));

            if (string.IsNullOrWhiteSpace(EditableModel.Name))
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("Generic", "ErrorName1"));
            }
            else if (EditableModel.Name.Length < 3)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("Generic", "ErrorName2"));
            }
            else if (EditableModel.Name.Length >= 50)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("Generic", "ErrorName3"));
            }
        }

        // Validation method for Description
        private void ValidateDescription()
        {
            ClearErrors(nameof(EditableModel.Description));

            if (!string.IsNullOrWhiteSpace(EditableModel.Description) && EditableModel.Description.Length > 250)
            {
                AddError(nameof(EditableModel.Description), LocalizationHelper.GetString("VehicleModels", "ErrorDescription"));
            }
        }

        // Validation method for EngineSize
        private void ValidateEngineSize()
        {
            ClearErrors(nameof(EditableModel.EngineSize));

            if (EditableModel.EngineSize == null)
            {
                AddError(nameof(EditableModel.EngineSize), LocalizationHelper.GetString("VehicleModels", "ErrorEngineSize1"));
            }
            else if (EditableModel.EngineSize <= 0)
            {
                AddError(nameof(EditableModel.EngineSize), LocalizationHelper.GetString("VehicleModels", "ErrorEngineSize2"));
            }
            else if (EditableModel.EngineSize > 10.0)
            {
                AddError(nameof(EditableModel.EngineSize), LocalizationHelper.GetString("VehicleModels", "ErrorEngineSize3"));
            }
        }

        // Validation method for HorsePower
        private void ValidateHorsePower()
        {
            ClearErrors(nameof(EditableModel.HorsePower));

            if (EditableModel.HorsePower == null)
            {
                AddError(nameof(EditableModel.HorsePower), LocalizationHelper.GetString("VehicleModels", "ErrorHorsePower1"));
            }
            else if (EditableModel.HorsePower <= 0)
            {
                AddError(nameof(EditableModel.HorsePower), LocalizationHelper.GetString("VehicleModels", "ErrorHorsePower2"));
            }
            else if (EditableModel.HorsePower > 1000)
            {
                AddError(nameof(EditableModel.HorsePower), LocalizationHelper.GetString("VehicleModels", "ErrorHorsePower3"));
            }
        }

        // Validation method for FuelType
        private void ValidateFuelType()
        {
            ClearErrors(nameof(EditableModel.FuelType));

            if (string.IsNullOrWhiteSpace(EditableModel.FuelType))
            {
                AddError(nameof(EditableModel.FuelType), LocalizationHelper.GetString("VehicleModels", "ErrorFuelType1"));
            }
            else if (!new[] { "Petrol", "Diesel", "Electric", "Hybrid" }.Contains(EditableModel.FuelType))
            {
                AddError(nameof(EditableModel.FuelType), LocalizationHelper.GetString("VehicleModels", "ErrorFuelType2"));
            }
        }

        // Validation method for VehicleBrand
        private void ValidateVehicleBrand()
        {
            ClearErrors(nameof(EditableModel.VehicleBrand));

            if (EditableModel.VehicleBrand == null)
            {
                AddError(nameof(EditableModel.VehicleBrand), LocalizationHelper.GetString("VehicleModels", "ErrorVehicleBrand1"));
            }
        }

        #endregion
    }
}

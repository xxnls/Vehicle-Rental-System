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
        public SelectorDialogParameters SelectVehicleBrandIdParameters { get; set; }

        public VehicleModelsViewModel() : base("VehicleModels", LocalizationHelper.GetString("VehicleModels", "DisplayName"))
        {
            SelectVehicleBrandIdParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(VehicleBrandsViewModel),
                SelectorView = new VehicleBrandsView(),
                TargetProperty = result => EditableModel.VehicleBrandId = (int)result,
                PropertyForSelection = "VehicleBrandId",
                Title = LocalizationHelper.GetString("VehicleModels", "SelectVehicleBrandTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(() => DeleteModelAsync(EditableModel.VehicleModelId));

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.Description), ValidateDescription},
                { nameof(EditableModel.EngineSize), ValidateEngineSize },
                { nameof(EditableModel.HorsePower), ValidateHorsePower },
                { nameof(EditableModel.FuelType), ValidateFuelType },
                { nameof(EditableModel.VehicleBrandId), ValidateVehicleBrandId }
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new VehicleModelDto
            {
                VehicleBrandId = EditableModel.VehicleBrandId,
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                EngineSize = EditableModel.EngineSize,
                HorsePower = EditableModel.HorsePower,
                FuelType = EditableModel.FuelType
            };

            await CreateModelAsync(model);
        }

        public async Task UpdateModelAsync()
        {
            var id = EditableModel.VehicleModelId;

            var model = new VehicleModelDto
            {
                VehicleModelId = EditableModel.VehicleModelId,
                VehicleBrandId = EditableModel.VehicleBrandId,
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                EngineSize = EditableModel.EngineSize,
                HorsePower = EditableModel.HorsePower,
                FuelType = EditableModel.FuelType
            };
            await UpdateModelAsync(id, model);
        }

        #endregion

        #region Validation
        private void ValidateName()
        {
            ValidateProperty(nameof(EditableModel.Name),
                () => !string.IsNullOrWhiteSpace(EditableModel.Name) && EditableModel.Name.Length >= 3,
                LocalizationHelper.GetString("VehicleModels", "ErrorName"));
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(EditableModel.Description),
                () => string.IsNullOrWhiteSpace(EditableModel.Description) || EditableModel.Description.Length <= 250,
                LocalizationHelper.GetString("VehicleModels", "ErrorDescription"));
        }

        private void ValidateEngineSize()
        {
            //ValidateProperty(nameof(EditableModel.EngineSize),
            //    () =>
            //    {
            //        // Check if EngineSize is a valid number and greater than 0
            //        return EditableModel.EngineSize.HasValue && EditableModel.EngineSize.Value > 0;
            //    },
            //    LocalizationHelper.GetString("VehicleModels", "ErrorEngineSize"));
        }

        private void ValidateHorsePower()
        {
            //ValidateProperty(nameof(EditableModel.HorsePower),
            //    () =>
            //    {
            //        // Check if HorsePower is a valid number and greater than 0
            //        return EditableModel.HorsePower.HasValue && EditableModel.HorsePower.Value > 0;
            //    },
            //    LocalizationHelper.GetString("VehicleModels", "ErrorHorsePower"));
        }

        private void ValidateFuelType()
        {
            ValidateProperty(nameof(EditableModel.FuelType),
                () => !string.IsNullOrWhiteSpace(EditableModel.FuelType) && EditableModel.FuelType.Length <= 10,
                LocalizationHelper.GetString("VehicleModels", "ErrorFuelType"));
        }

        private void ValidateVehicleBrandId()
        {
            ValidateProperty(nameof(EditableModel.VehicleBrandId),
                () => EditableModel.VehicleBrandId > 0,
                LocalizationHelper.GetString("VehicleModels", "ErrorVehicleBrandId"));
        }


        #endregion
    }
}

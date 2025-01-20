using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Vehicles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehicleTypesViewModel : BaseListViewModel<VehicleTypeDto>, IListViewModel
    {
        public VehicleTypesViewModel()
            : base("VehicleTypes", LocalizationHelper.GetString("VehicleTypes", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.VehicleTypeId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.Description), ValidateDescription },
                { nameof(EditableModel.BaseDailyRate), ValidateBaseDailyRate },
                { nameof(EditableModel.BaseWeeklyRate), ValidateBaseWeeklyRate },
                { nameof(EditableModel.BaseDeposit), ValidateBaseDeposit },
                { nameof(EditableModel.RequiredLicenseType), ValidateRequiredLicenseType }
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            var model = new VehicleTypeDto
            {
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                BaseDailyRate = EditableModel.BaseDailyRate,
                BaseWeeklyRate = EditableModel.BaseWeeklyRate,
                BaseDeposit = EditableModel.BaseDeposit,
                RequiredLicenseType = EditableModel.RequiredLicenseType
            };

            await CreateModelAsync(model);
        }

        public async Task UpdateModelAsync()
        {
            var id = EditableModel.VehicleTypeId;

            var model = new VehicleTypeDto
            {
                VehicleTypeId = EditableModel.VehicleTypeId,
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                BaseDailyRate = EditableModel.BaseDailyRate,
                BaseWeeklyRate = EditableModel.BaseWeeklyRate,
                BaseDeposit = EditableModel.BaseDeposit,
                RequiredLicenseType = EditableModel.RequiredLicenseType
            };

            await UpdateModelAsync(id, model);
        }

        #endregion

        #region Validation

        private void ValidateName()
        {
            ValidateProperty(nameof(EditableModel.Name),
                () => !string.IsNullOrWhiteSpace(EditableModel.Name) && EditableModel.Name.Length >= 3,
                LocalizationHelper.GetString("VehicleTypes", "ErrorName"));
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(EditableModel.Description),
                () => string.IsNullOrWhiteSpace(EditableModel.Description) || EditableModel.Description.Length <= 300,
                LocalizationHelper.GetString("VehicleTypes", "ErrorDescription"));
        }

        private void ValidateBaseDailyRate()
        {
            ValidateProperty(nameof(EditableModel.BaseDailyRate),
                () => EditableModel.BaseDailyRate > 0,
                LocalizationHelper.GetString("VehicleTypes", "ErrorBaseDailyRate"));
        }

        private void ValidateBaseWeeklyRate()
        {
            ValidateProperty(nameof(EditableModel.BaseWeeklyRate),
                () => EditableModel.BaseWeeklyRate > 0,
                LocalizationHelper.GetString("VehicleTypes", "ErrorBaseWeeklyRate"));
        }

        private void ValidateBaseDeposit()
        {
            ValidateProperty(nameof(EditableModel.BaseDeposit),
                () => EditableModel.BaseDeposit > 0,
                LocalizationHelper.GetString("VehicleTypes", "ErrorBaseDeposit"));
        }

        private void ValidateRequiredLicenseType()
        {
            ValidateProperty(nameof(EditableModel.RequiredLicenseType),
                () => !string.IsNullOrWhiteSpace(EditableModel.RequiredLicenseType),
                LocalizationHelper.GetString("VehicleTypes", "ErrorRequiredLicenseType"));
        }

        #endregion
    }
}

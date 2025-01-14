using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace BackOffice.ViewModels
{
    public class VehicleBrandsViewModel : BaseListViewModel<VehicleBrandDto>, IListViewModel
    {
        public VehicleBrandsViewModel() : base("VehicleBrands", LocalizationHelper.GetString("VehicleBrands", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(() => DeleteModelAsync(EditableModel.VehicleBrandId));
        }

        #region Methods

        // Create a new vehicle brand
        public async Task CreateModelAsync()
        {
            var model = new VehicleBrandDto
            {
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                Website = EditableModel.Website,
                LogoUrl = EditableModel.LogoUrl
            };

            await CreateModelAsync(model);
        }

        // Update the selected vehicle brand
        public async Task UpdateModelAsync()
        {
            var id = EditableModel.VehicleBrandId;

            var model = new VehicleBrandDto
            {
                VehicleBrandId = EditableModel.VehicleBrandId, // There is a need for id because of check in controller
                Name = EditableModel.Name,
                Description = EditableModel.Description,
                Website = EditableModel.Website,
                LogoUrl = EditableModel.LogoUrl
            };

            await UpdateModelAsync(id, model);
        }

        #endregion

        #region Validation Rules

        protected override void ValidateEditableModel()
        {
            if (EditableModel == null) return;

            ValidateName();
            ValidateDescription();
            ValidateWebsite();
            ValidateLogoUrl();
        }

        private void ValidateName()
        {
            ClearErrors(nameof(EditableModel.Name));

            if (string.IsNullOrWhiteSpace(EditableModel.Name))
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("VehicleBrands", "ErrorName1"));
            }
            else if (EditableModel.Name.Length < 3)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("VehicleBrands", "ErrorName2"));
            }
            else if (EditableModel.Name.Length >= 50)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("VehicleBrands", "ErrorName3"));
            }
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(EditableModel.Description),
                () => string.IsNullOrWhiteSpace(EditableModel.Description) || EditableModel.Description.Length <= 250,
                LocalizationHelper.GetString("VehicleBrands", "ErrorDescription1"));
        }

        private void ValidateWebsite()
        {
            ValidateProperty(nameof(EditableModel.Website),
                () => !string.IsNullOrWhiteSpace(EditableModel.Website) && Uri.IsWellFormedUriString(EditableModel.Website, UriKind.Absolute),
                LocalizationHelper.GetString("VehicleBrands", "ErrorWebsiteURL"));
        }

        private void ValidateLogoUrl()
        {
            ValidateProperty(nameof(EditableModel.LogoUrl),
                () => !string.IsNullOrWhiteSpace(EditableModel.LogoUrl) && Uri.IsWellFormedUriString(EditableModel.LogoUrl, UriKind.Absolute),
                LocalizationHelper.GetString("VehicleBrands", "ErrorLogoURL"));
        }

        #endregion
    }
}

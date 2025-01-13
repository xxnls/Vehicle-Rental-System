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
    public class VehicleBrandsViewModel : BaseListViewModel<VehicleBrandDto>
    {
        public VehicleBrandsViewModel() : base("VehicleBrands", "Vehicle Brands")
        {
            AddVehicleBrandCommand = new RelayCommand(async () => await AddVehicleBrandAsync());
            UpdateVehicleBrandCommand = new RelayCommand(async () => await UpdateVehicleBrandAsync());
            DeleteVehicleBrandCommand = new RelayCommand(async () => await DeleteVehicleBrandAsync());

            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            LoadNextPageCommand = new RelayCommand(async () => await LoadNextPageAsync(), () => CanLoadNextPage);
            LoadPreviousPageCommand = new RelayCommand(async () => await LoadPreviousPageAsync(), () => CanLoadPreviousPage);
            SearchCommand = new AsyncRelayCommand<string>(LoadModelsAsync);

            LoadModelsAsync();
        }

        #region Commands

        public ICommand AddVehicleBrandCommand { get; }
        public ICommand UpdateVehicleBrandCommand { get; }
        public ICommand DeleteVehicleBrandCommand { get; }

        #endregion

        #region Methods

        // Add a new vehicle brand
        private async Task AddVehicleBrandAsync()
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null)
                {
                    UpdateStatus("Error while adding a new model.");
                    return;
                }

                var NewVehicleBrand = new VehicleBrandDto()
                {
                    Name = EditableModel.Name,
                    Description = EditableModel.Description,
                    Website = EditableModel.Website,
                    LogoUrl = EditableModel.LogoUrl
                };

                await ApiClient.PostAsync<VehicleBrandDto, VehicleBrandDto>("VehicleBrands", NewVehicleBrand);
                UpdateStatus("Vehicle brand added successfully.");
                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error adding vehicle brand: {ex.Message}");
            }
            finally
            {
                IsBusy = false;

                SwitchViewMode(ViewMode.List);
            }
        }

        // Update the selected vehicle brand
        private async Task UpdateVehicleBrandAsync()
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null)
                {
                    UpdateStatus("Please select a vehicle brand to edit.");
                    return;
                }

                var updatedBrand = new VehicleBrandDto
                {
                    Name = EditableModel.Name,
                    Description = EditableModel.Description,
                    Website = EditableModel.Website,
                    LogoUrl = EditableModel.LogoUrl
                };

                await ApiClient.PutAsync($"VehicleBrands/{EditableModel.VehicleBrandId}", updatedBrand);
                UpdateStatus("Vehicle brand updated successfully.");
                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error updating vehicle brand: {ex.Message}");
            }
            finally
            {
                IsBusy = false;

                SwitchViewMode(ViewMode.List);
            }
        }

        // Delete a vehicle brand
        private async Task DeleteVehicleBrandAsync()
        {
            if (EditableModel == null)
            {
                UpdateStatus("Please select a vehicle brand to delete.");
                return;
            }

            try
            {
                IsBusy = true;
                await ApiClient.DeleteAsync($"VehicleBrands/{EditableModel.VehicleBrandId}");
                UpdateStatus("Vehicle brand deleted successfully.");
                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error deleting vehicle brand: {ex.Message}");
            }
            finally
            {   
                IsBusy = false;
            }
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
                AddError(nameof(EditableModel.Name), "Name cannot be empty.");
            }
            else if (EditableModel.Name.Length < 3)
            {
                AddError(nameof(EditableModel.Name), "Name must be at least 3 characters long.");
            }
            else if (EditableModel.Name.Length >= 50)
            {
                AddError(nameof(EditableModel.Name), "Name cannot exceed 50 characters.");
            }
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(EditableModel.Description),
                () => string.IsNullOrWhiteSpace(EditableModel.Description) || EditableModel.Description.Length <= 250,
                "Description cannot exceed 250 characters.");
        }

        private void ValidateWebsite()
        {
            ValidateProperty(nameof(EditableModel.Website),
                () => !string.IsNullOrWhiteSpace(EditableModel.Website) && Uri.IsWellFormedUriString(EditableModel.Website, UriKind.Absolute),
                "Website URL is required and must be a valid URL.");
        }

        private void ValidateLogoUrl()
        {
            ValidateProperty(nameof(EditableModel.LogoUrl),
                () => !string.IsNullOrWhiteSpace(EditableModel.LogoUrl) && Uri.IsWellFormedUriString(EditableModel.LogoUrl, UriKind.Absolute),
                "Logo URL is required and must be a valid URL.");
        }

        #endregion
    }
}

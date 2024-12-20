using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;
using BackOffice.Helpers;
using BackOffice.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace BackOffice.ViewModels
{
    public class VehicleBrandsViewModel : BaseListViewModel<RVehicleBrandDTO>
    {
        public VehicleBrandsViewModel() : base("VehicleBrands")
        {
            AddVehicleBrandCommand = new RelayCommand(async () => await AddVehicleBrandAsync());
            UpdateVehicleBrandCommand = new RelayCommand(async () => await UpdateVehicleBrandAsync());
            DeleteVehicleBrandCommand = new RelayCommand(async () => await DeleteVehicleBrandAsync());
            SwitchToCreateModeCommand = new RelayCommand(SwitchToCreateMode);
            SwitchToEditModeCommand = new RelayCommand(SwitchToEditMode);
            SwitchToListModeCommand = new RelayCommand(Cancel);

            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            LoadNextPageCommand = new RelayCommand(async () => await LoadNextPageAsync(), () => CanLoadNextPage);
            LoadPreviousPageCommand = new RelayCommand(async () => await LoadPreviousPageAsync(), () => CanLoadPreviousPage);
            SearchCommand = new AsyncRelayCommand<string>(LoadModelsAsync);

            LoadModelsAsync();
        }

        #region Properties & Fields

        // Selected vehicle brand for editing or details
        private RVehicleBrandDTO? _selectedVehicleBrand;
        public RVehicleBrandDTO? SelectedVehicleBrand
        {
            get => _selectedVehicleBrand;
            set
            {
                _selectedVehicleBrand = value;
                OnPropertyChanged(nameof(SelectedVehicleBrand));

                // Populate editable properties when selecting an item
                if (_selectedVehicleBrand != null)
                {
                    Name = _selectedVehicleBrand.Name;
                    Description = _selectedVehicleBrand.Description;
                    Website = _selectedVehicleBrand.Website;
                    LogoUrl = _selectedVehicleBrand.LogoUrl;
                }
            }
        }

        // Properties for binding input fields
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ValidateName();
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
                ValidateDescription();
            }
        }

        private string? _website;
        public string? Website
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged(nameof(Website));
                ValidateWebsite();
            }
        }

        private string? _logoUrl;
        public string? LogoUrl
        {
            get => _logoUrl;
            set
            {
                _logoUrl = value;
                OnPropertyChanged(nameof(LogoUrl));
                ValidateLogoUrl();
            }
        }

        #endregion

        #region Commands

        public ICommand AddVehicleBrandCommand { get; }
        public ICommand UpdateVehicleBrandCommand { get; }
        public ICommand DeleteVehicleBrandCommand { get; }
        public ICommand SwitchToListModeCommand { get; }
        public ICommand SwitchToCreateModeCommand { get; }
        public ICommand SwitchToEditModeCommand { get; }

        #endregion

        #region Methods

        // Clear all input fields
        private void ClearInputFields()
        {
            Name = string.Empty;
            Description = null;
            Website = null;
            LogoUrl = null;
        }

        // Cancel the current operation and switch back to list mode
        private void Cancel()
        {
            IsCreating = false;
            IsEditing = false;
            IsListVisible = true;

            ClearInputFields();
        }

        // Switch to create mode
        private void SwitchToCreateMode()
        {
            ClearInputFields();

            IsEditing = false;
            IsListVisible = false;
            IsCreating = true;

            UpdateStatus("Switched to create mode.");
        }

        // Switch to edit mode
        private void SwitchToEditMode()
        {
            if (SelectedVehicleBrand == null)
            {
                UpdateStatus("Please select an object to edit.");
                return;
            }

            IsListVisible = false;
            IsCreating = false;
            IsEditing = true;

            UpdateStatus("Switched to edit mode.");
        }



        // Add a new vehicle brand
        private async Task AddVehicleBrandAsync()
        {
            try
            {
                IsBusy = true;

                var NewVehicleBrand = new CUVehicleBrandDTO
                {
                    Name = Name,
                    Description = Description,
                    Website = Website,
                    LogoUrl = LogoUrl
                };

                await ApiClient.PostAsync<CUVehicleBrandDTO, RVehicleBrandDTO>("VehicleBrands", NewVehicleBrand);
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

                ClearInputFields();

                Cancel();
            }
        }

        // Update the selected vehicle brand
        private async Task UpdateVehicleBrandAsync()
        {
            try
            {
                IsBusy = true;

                var updatedBrand = new CUVehicleBrandDTO
                {
                    Name = Name,
                    Description = Description,
                    Website = Website,
                    LogoUrl = LogoUrl
                };

                await ApiClient.PutAsync($"VehicleBrands/{SelectedVehicleBrand.VehicleBrandId}", updatedBrand);
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
                Cancel();
            }
        }

        // Delete a vehicle brand
        private async Task DeleteVehicleBrandAsync()
        {
            if (SelectedVehicleBrand == null)
            {
                UpdateStatus("Please select a vehicle brand to delete.");
                return;
            }

            try
            {
                IsBusy = true;
                await ApiClient.DeleteAsync($"VehicleBrands/{SelectedVehicleBrand.VehicleBrandId}");
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

        private void ValidateName()
        {
            ClearErrors(nameof(Name));

            if (string.IsNullOrWhiteSpace(Name))
            {
                AddError(nameof(Name), "Name cannot be empty.");
            }
            else if (Name.Length < 3)
            {
                AddError(nameof(Name), "Name must be at least 3 characters long.");
            }
            else if (Name.Length >= 50)
            {
                AddError(nameof(Name), "Name cannot exceed 50 characters.");
            }
        }

        private void ValidateDescription()
        {
            ValidateProperty(nameof(Description),
                () => string.IsNullOrWhiteSpace(Description) || Description.Length <= 250,
                "Description cannot exceed 250 characters.");
        }

        private void ValidateWebsite()
        {
            ValidateProperty(nameof(Website),
                () => string.IsNullOrWhiteSpace(Website) || Uri.IsWellFormedUriString(Website, UriKind.Absolute),
                "Invalid website URL.");
        }

        private void ValidateLogoUrl()
        {
            ValidateProperty(nameof(LogoUrl),
                () => string.IsNullOrWhiteSpace(LogoUrl) || Uri.IsWellFormedUriString(LogoUrl, UriKind.Absolute),
                "Invalid logo URL.");
        }

        #endregion
    }
}

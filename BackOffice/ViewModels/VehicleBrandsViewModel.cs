using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;
using BackOffice.Helpers;
using CommunityToolkit.Mvvm.Messaging;

namespace BackOffice.ViewModels
{
    public class VehicleBrandsViewModel : BaseListViewModel
    {
        private readonly ApiClient _apiClient;

        public VehicleBrandsViewModel()
        {
            _apiClient = new ApiClient();

            VehicleBrands = new ObservableCollection<RVehicleBrandDTO>();

            LoadVehicleBrandsCommand = new RelayCommand(async () => await LoadVehicleBrandsAsync());
            AddVehicleBrandCommand = new RelayCommand(async () => await AddVehicleBrandAsync());
            UpdateVehicleBrandCommand = new RelayCommand(async () => await UpdateVehicleBrandAsync());
            DeleteVehicleBrandCommand = new RelayCommand(async () => await DeleteVehicleBrandAsync());
            SwitchToCreateModeCommand = new RelayCommand(SwitchToCreateMode);
            SwitchToEditModeCommand = new RelayCommand(SwitchToEditMode);
            SwitchToListModeCommand = new RelayCommand(Cancel);

            LoadVehicleBrandsAsync();
        }

        #region Properties & Fields

        // Observable collection for displaying a list of vehicle brands
        public ObservableCollection<RVehicleBrandDTO> VehicleBrands { get; }

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
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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
            }
        }

        #endregion

        #region Commands

        public ICommand LoadVehicleBrandsCommand { get; }
        public ICommand AddVehicleBrandCommand { get; }
        public ICommand UpdateVehicleBrandCommand { get; }
        public ICommand DeleteVehicleBrandCommand { get; }
        public ICommand SwitchToListModeCommand { get; }
        public ICommand SwitchToCreateModeCommand { get; }
        public ICommand SwitchToEditModeCommand { get; }

        #endregion

        #region Methods

        // Load all vehicle brands
        private async Task LoadVehicleBrandsAsync()
        {
            try
            {
                IsBusy = true;
                var brands = await _apiClient.GetAsync<ObservableCollection<RVehicleBrandDTO>>("VehicleBrands");
                VehicleBrands.Clear();
                foreach (var brand in brands)
                {
                    VehicleBrands.Add(brand);
                }

                UpdateStatus("Vehicle brands loaded successfully.");
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error loading vehicle brands: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Cancel the current operation and switch back to list mode
        private void Cancel()
        {
            IsCreating = false;
            IsEditing = false;
            IsListVisible = true;

            UpdateStatus("Switched back to list mode.");
        }

        // Switch to create mode
        private void SwitchToCreateMode()
        {
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

                var newBrand = new CUVehicleBrandDTO
                {
                    Name = Name,
                    Description = Description,
                    Website = Website,
                    LogoUrl = LogoUrl
                };

                await _apiClient.PostAsync<CUVehicleBrandDTO, RVehicleBrandDTO>("VehicleBrands", newBrand);
                UpdateStatus("Vehicle brand added successfully.");
                await LoadVehicleBrandsAsync();
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error adding vehicle brand: {ex.Message}");
            }
            finally
            {
                IsBusy = false;

                // Reset form fields for a new entry
                Name = string.Empty;
                Description = null;
                Website = null;
                LogoUrl = null;

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

                await _apiClient.PutAsync($"VehicleBrands/{SelectedVehicleBrand.VehicleBrandId}", updatedBrand);
                UpdateStatus("Vehicle brand updated successfully.");
                await LoadVehicleBrandsAsync();
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error updating vehicle brand: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
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
                await _apiClient.DeleteAsync($"VehicleBrands/{SelectedVehicleBrand.VehicleBrandId}");
                UpdateStatus("Vehicle brand deleted successfully.");
                await LoadVehicleBrandsAsync();
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
    }
}

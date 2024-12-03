using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using BackOffice.Models.Vehicles.VehicleBrands.DTOs;

namespace BackOffice.ViewModels
{
    public class VehicleBrandsViewModel : BaseViewModel
    {
        private readonly ApiClient _apiClient;

        public VehicleBrandsViewModel()
        {
            _apiClient = new ApiClient();
            VehicleBrands = new ObservableCollection<RVehicleBrandDTO>();
            LoadVehicleBrandsCommand = new RelayCommand(async () => await LoadVehicleBrandsAsync());
            AddVehicleBrandCommand = new RelayCommand(async () => await AddVehicleBrandAsync());
            UpdateVehicleBrandCommand = new RelayCommand(async () => await UpdateVehicleBrandAsync());
            DeleteVehicleBrandCommand = new RelayCommand<int>(async (id) => await DeleteVehicleBrandAsync(id));

            LoadVehicleBrandsAsync();
        }

        #region Properties

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
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }

        #endregion

        #region Commands

        public ICommand LoadVehicleBrandsCommand { get; }
        public ICommand AddVehicleBrandCommand { get; }
        public ICommand UpdateVehicleBrandCommand { get; }
        public ICommand DeleteVehicleBrandCommand { get; }

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
                StatusMessage = "Vehicle brands loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading vehicle brands: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
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
                StatusMessage = "Vehicle brand added successfully.";
                await LoadVehicleBrandsAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding vehicle brand: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Update the selected vehicle brand
        private async Task UpdateVehicleBrandAsync()
        {
            //if (SelectedVehicleBrand == null)
            //{
            //    StatusMessage = "Please select a vehicle brand to update.";
            //    return;
            //}

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
                StatusMessage = "Vehicle brand updated successfully.";
                await LoadVehicleBrandsAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating vehicle brand: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Delete a vehicle brand
        private async Task DeleteVehicleBrandAsync(int id)
        {
            try
            {
                IsBusy = true;
                await _apiClient.DeleteAsync($"VehicleBrands/{id}");
                StatusMessage = "Vehicle brand deleted successfully.";
                await LoadVehicleBrandsAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting vehicle brand: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}

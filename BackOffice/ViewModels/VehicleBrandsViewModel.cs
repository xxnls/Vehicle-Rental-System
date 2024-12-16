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
    public class VehicleBrandsViewModel : BaseListViewModel
    {
        private readonly ApiClient _apiClient;

        public VehicleBrandsViewModel()
        {
            _apiClient = new ApiClient();

            VehicleBrands = new ObservableCollection<RVehicleBrandDTO>();

            LoadVehicleBrandsCommand = new RelayCommand(async () => await LoadVehicleBrandsAsync());
            NextPageCommand = new RelayCommand(async () => await LoadNextPageAsync(), () => CanLoadNextPage);
            PreviousPageCommand = new RelayCommand(async () => await LoadPreviousPageAsync(), () => CanLoadPreviousPage);
            AddVehicleBrandCommand = new RelayCommand(async () => await AddVehicleBrandAsync());
            UpdateVehicleBrandCommand = new RelayCommand(async () => await UpdateVehicleBrandAsync());
            DeleteVehicleBrandCommand = new RelayCommand(async () => await DeleteVehicleBrandAsync());
            SwitchToCreateModeCommand = new RelayCommand(SwitchToCreateMode);
            SwitchToEditModeCommand = new RelayCommand(SwitchToEditMode);
            SwitchToListModeCommand = new RelayCommand(Cancel);
            SearchCommand = new AsyncRelayCommand<string>(Search);

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

        public ICommand LoadVehicleBrandsCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand AddVehicleBrandCommand { get; }
        public ICommand UpdateVehicleBrandCommand { get; }
        public ICommand DeleteVehicleBrandCommand { get; }
        public ICommand SwitchToListModeCommand { get; }
        public ICommand SwitchToCreateModeCommand { get; }
        public ICommand SwitchToEditModeCommand { get; }
        public ICommand SearchCommand { get; }

        #endregion

        #region Methods

        // Load all vehicle brands
        private async Task LoadVehicleBrandsAsync()
        {
            try
            {
                IsBusy = true;
                string endpoint = $"VehicleBrands?page={CurrentPage}&pageSize={PageSize}";
                var brands = await _apiClient.GetAsync<PaginatedResult<RVehicleBrandDTO>>(endpoint);
                VehicleBrands.Clear();
                foreach (var brand in brands.Items)
                {
                    VehicleBrands.Add(brand);
                }

                //UpdateStatus("Vehicle brands loaded successfully.");
                TotalItemCount = brands.TotalItemCount;
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

        #region Pagination
        private async Task LoadNextPageAsync()
        {
            if (CurrentPage < (TotalItemCount + PageSize - 1) / PageSize) // Calculate total pages
            {
                CurrentPage++;
                await LoadVehicleBrandsAsync();
            }
        }

        private async Task LoadPreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadVehicleBrandsAsync();
            }
        }

        #endregion

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

        // Search for vehicle brands
        private async Task Search(string? searchInput)
        {
            try
            {
                IsBusy = true;
                var brands = await _apiClient.GetAsync<ObservableCollection<RVehicleBrandDTO>>("VehicleBrands?search=" + searchInput);
                VehicleBrands.Clear();
                foreach (var brand in brands)
                {
                    VehicleBrands.Add(brand);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error searching vehicle brands: {ex.Message}");
            }
            finally
            {
                UpdateStatus("Search completed (" + searchInput + ").");
                IsBusy = false;
            }
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

                await _apiClient.PostAsync<CUVehicleBrandDTO, RVehicleBrandDTO>("VehicleBrands", NewVehicleBrand);
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

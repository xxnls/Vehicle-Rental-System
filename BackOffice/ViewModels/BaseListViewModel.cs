using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Models;
using BackOffice.Properties;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels
{
    public class BaseListViewModel<T> : BaseViewModel, INotifyDataErrorInfo
    {
        public BaseListViewModel()
        {
            // Set the initial visibility
            IsListVisible = true;
            IsCreating = false;
            IsEditing = false;

            _apiClient = new ApiClient();
            Models = new ObservableCollection<T>();

        }

        #region Properties & Fields

        protected readonly ApiClient _apiClient;

        // Observable collection for displaying a list of vehicle brands
        public ObservableCollection<T> Models { get; }
        protected string EndPointName = string.Empty;

        private readonly Dictionary<string, List<string>> _validationErrors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _validationErrors.Count > 0;

        // Visibility properties for switching between list, create, and edit modes
        private bool _isListVisible;
        public bool IsListVisible
        {
            get => _isListVisible;
            set
            {
                _isListVisible = value;
                OnPropertyChanged(nameof(IsListVisible));
            }
        }

        private bool _isCreating;
        public bool IsCreating
        {
            get => _isCreating;
            set
            {
                _isCreating = value;
                OnPropertyChanged(nameof(IsCreating));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        #endregion

        #region Commands

        public ICommand LoadModelsCommand { get; set; }
        public ICommand LoadNextPageCommand { get; set; }
        public ICommand LoadPreviousPageCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        #endregion

        #region Core Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task LoadModelsAsync()
        {
            try
            {
                IsBusy = true;
                string endpoint = $"{EndPointName}?page={CurrentPage}&pageSize={PageSize}";
                var results = await _apiClient.GetAsync<PaginatedResult<T>>(endpoint);

                Models.Clear();
                foreach (var result in results.Items)
                {
                    Models.Add(result);
                }

                //UpdateStatus("Vehicle brands loaded successfully.");
                TotalItemCount = results.TotalItemCount;
            }
            catch (Exception ex)
            {
                // TODO: DO ERROR DIALOG
                UpdateStatus($"Error while loading models: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Search for vehicle brands
        protected async Task Search(string? searchInput)
        {
            try
            {
                IsBusy = true;
                string endpoint = $"{EndPointName}?search={searchInput}&page={CurrentPage}&pageSize={PageSize}";
                var results = await _apiClient.GetAsync<PaginatedResult<T>>(endpoint);

                Models.Clear();
                foreach (var result in results.Items)
                {
                    Models.Add(result);
                }

                TotalItemCount = results.TotalItemCount;

                UpdateStatus($"Search completed for '{searchInput}' ({Models.Count} results).");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error searching while models: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected async Task LoadNextPageAsync()
        {
            if (CurrentPage < (TotalItemCount + PageSize - 1) / PageSize) // Calculate total pages
            {
                CurrentPage++;
                await LoadModelsAsync();
            }
        }

        protected async Task LoadPreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadModelsAsync();
            }
        }


        #endregion

        #region Pagination

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdatePaginationState();
            }
        }

        private int _pageSize = Settings.Default.PageSize;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                OnPropertyChanged(nameof(PageSize));
                UpdatePaginationState();
            }
        }

        private int _totalItemCount;
        public int TotalItemCount
        {
            get => _totalItemCount;
            set
            {
                _totalItemCount = value;
                OnPropertyChanged(nameof(TotalItemCount));
                UpdatePaginationState();
            }
        }

        private bool _canLoadNextPage = true;
        public bool CanLoadNextPage
        {
            get => _canLoadNextPage;
            set
            {
                _canLoadNextPage = value; 
                OnPropertyChanged(nameof(CanLoadNextPage));
            }
        }

        private bool _canLoadPreviousPage = true;
        public bool CanLoadPreviousPage
        {
            get => _canLoadPreviousPage;
            set
            {
                _canLoadPreviousPage = value;
                OnPropertyChanged(nameof(CanLoadPreviousPage));
            }
        }

        private void UpdatePaginationState()
        {
            CanLoadNextPage = CurrentPage < (TotalItemCount + PageSize - 1) / PageSize;
            CanLoadPreviousPage = CurrentPage > 1;
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _validationErrors.ContainsKey(propertyName))
                return _validationErrors[propertyName];

            return Enumerable.Empty<string>();
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            if (!_validationErrors.ContainsKey(propertyName))
                _validationErrors[propertyName] = new List<string>();

            if (!_validationErrors[propertyName].Contains(errorMessage))
            {
                _validationErrors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
                OnPropertyChanged(nameof(HasErrors)); // Notify UI of HasErrors change
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_validationErrors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(nameof(HasErrors)); // Notify UI of HasErrors change
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates a single property using a validation function.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="validateFunc">The validation function to check the property.</param>
        protected void ValidateProperty(string propertyName, Func<bool> validateFunc, string errorMessage)
        {
            if (validateFunc())
            {
                ClearErrors(propertyName);
            }
            else
            {
                AddError(propertyName, errorMessage);
            }
        }

        /// <summary>
        /// Clears all validation errors.
        /// </summary>
        protected void ClearAllErrors()
        {
            var propertyNames = new List<string>(_validationErrors.Keys);
            foreach (var propertyName in propertyNames)
            {
                ClearErrors(propertyName);
            }
        }

        #endregion
    }
}

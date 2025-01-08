using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Input;
using BackOffice.Models;
using BackOffice.Properties;
using BackOffice.Services;

namespace BackOffice.ViewModels
{
    public class BaseListViewModel<T> : BaseViewModel, INotifyDataErrorInfo
    {
        public BaseListViewModel(string endPointName)
        {
            // Set the initial visibility
            IsListVisible = true;
            IsCreating = false;
            IsEditing = false;
            IsFiltering = false;

            EndPointName = endPointName;

            ApiClient = new ApiClient();
            Models = [];

        }

        #region Properties & Fields

        protected readonly ApiClient ApiClient;

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
                OnPropertyChanged();
            }
        }

        private bool _isCreating;
        public bool IsCreating
        {
            get => _isCreating;
            set
            {
                _isCreating = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        private bool _isFiltering;
        public bool IsFiltering
        {
            get => _isFiltering;
            set
            {
                _isFiltering = value;
                OnPropertyChanged();
            }
        }

        private string? _currentSearchInput;
        public string? CurrentSearchInput
        {
            get => _currentSearchInput;
            set
            {
                _currentSearchInput = value;
                OnPropertyChanged();
            }
        }

        #region Filter

        // Filters for created date
        private DateTime? _createdBefore;
        public DateTime? CreatedBefore
        {
            get => _createdBefore;
            set
            {
                _createdBefore = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _createdAfter;
        public DateTime? CreatedAfter
        {
            get => _createdAfter;
            set
            {
                _createdAfter = value;
                OnPropertyChanged();
            }
        }

        // Filters for modified date
        private DateTime? _modifiedBefore;
        public DateTime? ModifiedBefore
        {
            get => _modifiedBefore;
            set
            {
                _modifiedBefore = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _modifiedAfter;
        public DateTime? ModifiedAfter
        {
            get => _modifiedAfter;
            set
            {
                _modifiedAfter = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #endregion

        #region Commands

        public ICommand? LoadModelsCommand { get; set; }
        public ICommand? LoadNextPageCommand { get; set; }
        public ICommand? LoadPreviousPageCommand { get; set; }
        public ICommand? SearchCommand { get; set; }

        #endregion

        #region Core Methods

        /// <summary>
        /// Loads data models asynchronously from the API based on the current page, page size, and optional search input.
        /// Updates the <see cref="Models"/> collection with the fetched data and sets the <see cref="TotalItemCount"/> property.
        /// If a search input is provided, the API request includes it to filter results.
        /// </summary>
        /// <param name="searchInput">
        /// Optional. A string used to filter the results. If <c>null</c> or whitespace, all data is loaded without filtering.
        /// </param>
        protected async Task LoadModelsAsync(string? searchInput = null)
        {
            try
            {
                IsBusy = true;
                var endpoint = string.Empty;
                CurrentSearchInput = searchInput;

                endpoint = string.IsNullOrWhiteSpace(searchInput)
                    ? $"{EndPointName}?page={CurrentPage}&pageSize={PageSize}"
                    : $"{EndPointName}?search={CurrentSearchInput}&page={CurrentPage}&pageSize={PageSize}";

                if (CreatedBefore.HasValue)
                    endpoint += $"&createdBefore={CreatedBefore.Value:yyyy-MM-dd}";
                if (CreatedAfter.HasValue)
                    endpoint += $"&createdAfter={CreatedAfter.Value:yyyy-MM-dd}";
                if (ModifiedBefore.HasValue)
                    endpoint += $"&modifiedBefore={ModifiedBefore.Value:yyyy-MM-dd}";
                if (ModifiedAfter.HasValue)
                    endpoint += $"&modifiedAfter={ModifiedAfter.Value:yyyy-MM-dd}";

                var results = await ApiClient.GetAsync<PaginatedResult<T>>(endpoint);
                Models.Clear();
                foreach (var result in results.Items)
                {
                    Models.Add(result);
                }

                TotalItemCount = results.TotalItemCount;

                UpdateStatus(string.IsNullOrWhiteSpace(searchInput)
                    ? "Vehicle brands loaded successfully."
                    : $"Search completed for '{CurrentSearchInput}' ({TotalItemCount} results).");
            }
            catch (Exception ex)
            {
                UpdateStatus(string.IsNullOrWhiteSpace(searchInput)
                    ? $"Error while loading models: {ex.Message}"
                    : $"Error searching while models: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }


        /// <summary>
        /// Updates the <see cref="Models"/> collection and pagination properties.
        /// Invokes <see cref="LoadModelsAsync"/> task to fetch data for next page.
        /// </summary>
        protected async Task LoadNextPageAsync()
        {
            if (CurrentPage < (TotalItemCount + PageSize - 1) / PageSize) // Calculate total pages
            {
                CurrentPage++;
                await LoadModelsAsync(CurrentSearchInput);
            }
        }

        /// <summary>
        /// Updates the <see cref="Models"/> collection and pagination properties.
        /// Invokes <see cref="LoadModelsAsync"/> task to fetch data for previous page.
        /// </summary>
        protected async Task LoadPreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadModelsAsync(CurrentSearchInput);
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
                OnPropertyChanged();
                UpdatePaginationState();
            }
        }

        private int _pageSize = Settings.Default.ModelsPerPage;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private bool _canLoadPreviousPage = true;
        public bool CanLoadPreviousPage
        {
            get => _canLoadPreviousPage;
            set
            {
                _canLoadPreviousPage = value;
                OnPropertyChanged();
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
        /// <param name="errorMessage">Error message to display in view.</param>
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

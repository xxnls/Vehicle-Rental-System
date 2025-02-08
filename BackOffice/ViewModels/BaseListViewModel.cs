using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Models;
using BackOffice.Properties;
using BackOffice.Services;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using BackOffice.Views;
using System.Windows.Data;
using BackOffice.Interfaces;

namespace BackOffice.ViewModels
{ 
    public class BaseListViewModel<T> : BaseViewModel, INotifyDataErrorInfo, IBaseListViewModel where T : BaseDtoModel, new()
    {
        public BaseListViewModel(string endPointName, string displayName)
        {
            // Set the initial visibility
            IsListVisible = true;
            IsCreating = false;
            IsEditing = false;
            IsFiltering = false;

            EndPointName = endPointName;
            DisplayName = displayName;

            ShowFilterOptionsCommand = new RelayCommand(ShowFilterOptions);
            ShowDeletedModelsCommand = new RelayCommand(ShowDeletedModels);
            SwitchToCreateModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Create));
            SwitchToEditModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Edit), () => EditableModel != null);
            SwitchToListModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.List));
            LoadModelsCommand = new RelayCommand(async () => await LoadModelsAsync());
            RestoreModelCommand = new AsyncRelayCommand<int>(
                async id => await RestoreModelAsync(id, EditableModel), id => EditableModel != null);
            LoadNextPageCommand = new RelayCommand(async () => await LoadNextPageAsync(), () => CanLoadNextPage);
            LoadPreviousPageCommand = new RelayCommand(async () => await LoadPreviousPageAsync(), () => CanLoadPreviousPage);
            ShowDetailedInfoCommand = new RelayCommand<DataGrid>(ShowDetailedInfo);
            SearchCommand = new AsyncRelayCommand<string>(LoadModelsAsync);
            ShowSelectorDialogCommand = new RelayCommand<SelectorDialogParameters>(ShowSelectorDialog);

            ApiClient = new ApiClient();
            Models = [];

            _ = LoadModelsAsync();
        }

        #region Properties & Fields

        protected readonly ApiClient ApiClient;

        // Observable collection for displaying a list of vehicle brands
        public ObservableCollection<T> Models { get; }
        protected string EndPointName;
        protected string DisplayName;
        protected Dictionary<string, Action> ValidationRules;
        private readonly Dictionary<string, List<string>> _validationErrors = new();
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _validationErrors.Count > 0;

        private T? _editableModel;
        public T? EditableModel
        {
            get => _editableModel;
            set
            {
                if (_editableModel is INotifyPropertyChanged oldModel)
                {
                    oldModel.PropertyChanged -= EditableModel_PropertyChanged;
                }

                _editableModel = value;

                if (_editableModel is INotifyPropertyChanged newModel)
                {
                    newModel.PropertyChanged += EditableModel_PropertyChanged;
                }

                OnPropertyChanged();
                DeleteModelCommand?.NotifyCanExecuteChanged();
                SwitchToEditModeCommand.NotifyCanExecuteChanged();
                RestoreModelCommand.NotifyCanExecuteChanged();
                ApproveRequestCommand?.NotifyCanExecuteChanged();
                RejectRequestCommand?.NotifyCanExecuteChanged();
                CancelRequestCommand?.NotifyCanExecuteChanged();
            }
        }

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

        public bool IsCreatingOrEditing => IsCreating || IsEditing;

        private bool _isCreating;
        public bool IsCreating
        {
            get => _isCreating;
            set
            {
                _isCreating = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCreatingOrEditing));
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
                OnPropertyChanged(nameof(IsCreatingOrEditing));
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

        private bool _showDeleted;
        public bool ShowDeleted
        {
            get => _showDeleted;
            set
            {
                _showDeleted = value;
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

        #endregion

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

        public ICommand LoadModelsCommand { get; set; }
        public ICommand? CreateModelCommand { get; set; }
        public ICommand? UpdateModelCommand { get; set; }
        public AsyncRelayCommand? DeleteModelCommand { get; set; }
        public ICommand LoadNextPageCommand { get; set; }
        public ICommand LoadPreviousPageCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SwitchToListModeCommand { get; }
        public ICommand SwitchToCreateModeCommand { get; set; }
        public RelayCommand SwitchToEditModeCommand { get; set; }
        public RelayCommand ShowFilterOptionsCommand { get; set; }
        public RelayCommand ShowDeletedModelsCommand { get; set; }
        public AsyncRelayCommand<int> RestoreModelCommand { get; set; }
        public ICommand ShowDetailedInfoCommand { get; }
        public ICommand ShowSelectorDialogCommand { get; }
        public AsyncRelayCommand? ApproveRequestCommand { get; set; }
        public AsyncRelayCommand? RejectRequestCommand { get; set; }
        public AsyncRelayCommand? CancelRequestCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Enum for switching between List, Create, and Edit view modes.
        /// </summary>
        public enum ViewMode
        {
            List,
            Create,
            Edit
        }

        /// <summary>
        /// Switches the view mode between List, Create, and Edit.
        /// </summary>
        /// <param name="mode">
        /// Chosen mode to switch to.
        /// </param>
        public void SwitchViewMode(ViewMode mode)
        {
            // Prevent entering Edit mode if no item is selected
            if (mode == ViewMode.Edit && EditableModel == null)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USEditSelect"));
                return;
            }

            // Create new editable model for Create mode
            if (mode == ViewMode.Create)
            {
                EditableModel = new T();
                ValidateEditableModel();
            }

            // Clear EditableModel when switching to List mode
            if (mode == ViewMode.List)
            {
                EditableModel = null;
            }

            IsCreating = mode == ViewMode.Create;
            IsEditing = mode == ViewMode.Edit;
            IsListVisible = mode == ViewMode.List;

            // UpdateStatus($"Switched to {mode.ToString().ToLower()} mode.");
        }

        /// <summary>
        /// Loads a list of models, optionally filtered by a search input. It also handles pagination and various filter options such as created and modified dates.
        /// </summary>
        /// <param name="searchInput">
        /// Optional. A string used to filter the results. If <c>null</c> or whitespace, all data is loaded without filtering.
        /// </param>
        protected virtual async Task LoadModelsAsync(string? searchInput = null)
        {
            try
            {
                IsBusy = true;

                CurrentSearchInput = searchInput;

                // Reset page number if search input is provided
                _ = !string.IsNullOrWhiteSpace(searchInput) ? CurrentPage = 1 : CurrentPage;

                var endpoint = string.IsNullOrWhiteSpace(searchInput)
                    ? $"{EndPointName}?page={CurrentPage}&pageSize={PageSize}"
                    : $"{EndPointName}?search={CurrentSearchInput}&page={CurrentPage}&pageSize={PageSize}";

                if (ShowDeleted)
                    endpoint += "&showDeleted=true";
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

                if (ShowDeleted)
                {
                    UpdateStatus(LocalizationHelper.GetString("Generic", "USLoadShowingDeleted") + $"{DisplayName}.");
                }
                else
                {
                    UpdateStatus(string.IsNullOrWhiteSpace(searchInput)
                        ? $"{DisplayName}" + LocalizationHelper.GetString("Generic", "USLoadSuccess")
                        : LocalizationHelper.GetString("Generic", "USLoadSearchCompleted1") + $"'{CurrentSearchInput}'"
                        + LocalizationHelper.GetString("Generic", "USLoadSearchCompleted2") + $"({TotalItemCount} "
                        + LocalizationHelper.GetString("Generic", "USLoadSearchCompleted3") + ").");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus(string.IsNullOrWhiteSpace(searchInput)
                    ? LocalizationHelper.GetString("Generic", "USLoadError1") + $"{ex.Message}"
                    : LocalizationHelper.GetString("Generic", "USLoadError2") + $"{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Creates a new model by sending a POST request to the API.
        /// After the creation, it reloads the list of models.
        /// </summary>
        /// <param name="model">
        /// The model object to be created. It is passed as a generic parameter to represent the model type.
        /// </param>
        protected async Task CreateModelAsync(T model)
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null)
                {
                    UpdateStatus(LocalizationHelper.GetString("Generic", "USCreateError1"));
                    return;
                }

                await ApiClient.PostAsync<T, T>(EndPointName, model);
                UpdateStatus(DisplayName + LocalizationHelper.GetString("Generic", "USCreateSuccess"));

                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USCreateError2") + $" {DisplayName}: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                SwitchViewMode(ViewMode.List);
            }
        }

        /// <summary>
        /// Updates an existing model by sending a PUT request to the API with the model's updated data.
        /// After the update, it reloads the list of models.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the model to be updated.
        /// </param>
        /// <param name="model">
        /// The updated model object to be sent to the API.
        /// </param>
        protected async Task UpdateModelAsync(int id, T model)
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null)
                {
                    UpdateStatus(LocalizationHelper.GetString("Generic", "USUpdateSelect"));
                    return;
                }

                await ApiClient.PutAsync($"{EndPointName}/{id}", model);
                UpdateStatus(DisplayName + LocalizationHelper.GetString("Generic", "USUpdateSuccess"));

                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USUpdateError") + $" {DisplayName}: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                SwitchViewMode(ViewMode.List);
            }
        }

        /// <summary>
        /// Deletes a model by sending a DELETE request to the API using the model's unique identifier.
        /// After the deletion, it reloads the list of models.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the model to be deleted.
        /// </param>
        protected async Task DeleteModelAsync(int? id)
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null)
                {
                    UpdateStatus(LocalizationHelper.GetString("Generic", "USDeleteSelect"));
                    return;
                }

                await ApiClient.DeleteAsync($"{EndPointName}/{id}");
                UpdateStatus(DisplayName + LocalizationHelper.GetString("Generic", "USDeleteSuccess"));

                await LoadModelsAsync();
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USDeleteError") + $" {DisplayName}: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Restores a deleted model by sending a PUT request to the API with the model's updated data.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the model to be restored.
        /// </param>
        /// <param name="model">
        /// The model object to be restored.
        /// </param>
        protected async Task RestoreModelAsync(int id, T? model)
        {
            if (model == null)
            {
                UpdateStatus(LocalizationHelper.GetString("Generic", "USRestoreSelect"));
                return;
            }

            model.DeletedDate = null;
            model.IsActive = true;

            await UpdateModelAsync(id, model);
        }

        /// <summary>
        /// Shows a dialog window with a DataGrid containing a list of selectable items.
        /// </summary>
        /// <param name="parameters">
        /// The parameters for the selector dialog.
        /// </param>
        /// <remarks>
        /// If the selected item has a property to be set, it will be set using the <see cref="SelectorDialogParameters.TargetProperty"/> action.
        /// </remarks>
        protected void ShowSelectorDialog(SelectorDialogParameters parameters)
        {
            // Ensure the type is a BaseViewModel
            if (!typeof(BaseViewModel).IsAssignableFrom(parameters.SelectorViewModelType))
                throw new ArgumentException($"Type {parameters.SelectorViewModelType} must inherit from BaseViewModel.", nameof(parameters.SelectorViewModelType));

            // Dynamically create an instance of the ViewModel
            var viewModel = (BaseViewModel)Activator.CreateInstance(parameters.SelectorViewModelType);

            var dialog = new SelectWindow
            {
                DataContext = viewModel,
                Title = parameters.Title
            };

            var originalGrid = parameters.SelectorView.FindName("MainDataGrid") as DataGrid;
            if (originalGrid == null) return;

            // Create a new grid
            var newGrid = new DataGrid
            {
                Style = originalGrid.Style,
                AutoGenerateColumns = originalGrid.AutoGenerateColumns
            };

            // Set up binding for ItemsSource
            var binding = new Binding("Models")
            {
                Source = viewModel,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            newGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);

            // Copy columns from original grid
            foreach (var column in originalGrid.Columns)
            {
                DataGridColumn newColumn = null;

                // Check if column is DataGridTextColumn
                if (column is DataGridTextColumn textColumn)
                {
                    newColumn = new DataGridTextColumn
                    {
                        Header = textColumn.Header,
                        Binding = textColumn.Binding
                    };
                }
                else if (column is DataGridCheckBoxColumn checkBoxColumn)
                {
                    newColumn = new DataGridCheckBoxColumn
                    {
                        Header = checkBoxColumn.Header,
                        Binding = checkBoxColumn.Binding
                    };
                }

                // Ensure newColumn is not null before adding it to newGrid
                if (newColumn != null)
                {
                    newGrid.Columns.Add(newColumn);
                }
            }

            // Add double click event
            newGrid.MouseDoubleClick += (sender, e) =>
            {
                if (newGrid.SelectedItem == null) return;

                var selectedItem = newGrid.SelectedItem;

                if (string.IsNullOrEmpty(parameters.PropertyForSelection))
                {
                    parameters.TargetProperty?.Invoke(selectedItem);
                }
                else
                {
                    var property = selectedItem.GetType().GetProperty(parameters.PropertyForSelection);
                    if (property != null)
                    {
                        var value = property.GetValue(selectedItem);
                        parameters.TargetProperty?.Invoke(value);
                    }
                }

                dialog.Close();
            };

            dialog.ContentPresenter.Content = newGrid;
            dialog.ShowDialog();
        }

        /// <summary>
        /// Updates the <see cref="Models"/> collection and pagination properties.
        /// Invokes <see cref="LoadModelsAsync"/> task to fetch data for next page.
        /// </summary>
        public async Task LoadNextPageAsync()
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
        public async Task LoadPreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadModelsAsync(CurrentSearchInput);
            }
        }

        protected void ValidateEditableModel()
        {
            if (EditableModel == null) return;

            foreach (var validate in ValidationRules.Values)
            {
                validate.Invoke();
            }
        }

        /// <summary>
        /// Toggles the visibility of the filter options.
        /// </summary>
        protected void ShowFilterOptions()
        {
            IsFiltering = !IsFiltering;
        }

        /// <summary>
        /// Toggles the visibility of deleted models.
        /// </summary>
        protected void ShowDeletedModels()
        {
            ShowDeleted = !ShowDeleted;
            CurrentPage = 1;

            _ = LoadModelsAsync();
        }

        /// <summary>
        /// Shows detailed information about a selected item in a DataGrid.
        /// </summary>
        /// <param name="dataGrid">
        /// The DataGrid containing the selected item.
        /// </param>
        private void ShowDetailedInfo(DataGrid dataGrid)
        {
            if (EditableModel == null) return;

            var dialog = new DetailedInfoWindow(dataGrid);
            dialog.Show();
        }

        public void UpdatePaginationState()
        {
            CanLoadPreviousPage = CurrentPage > 1;
            CanLoadNextPage = CurrentPage < (TotalItemCount + PageSize - 1) / PageSize;
        }

        private void EditableModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidateEditableModel();
        }

        #region Validation

        /// <summary>
        /// Gets the errors for a specific property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _validationErrors.TryGetValue(propertyName, out List<string>? value))
                return value;

            return Enumerable.Empty<string>();
        }

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
        /// Adds an error message to the validation errors collection.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that has an error.
        /// </param>
        /// <param name="errorMessage">
        /// The error message to display.
        /// </param>
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

        /// <summary>
        /// Clears the errors for a specific property.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to clear errors for.
        /// </param>
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

        #endregion

    }
}

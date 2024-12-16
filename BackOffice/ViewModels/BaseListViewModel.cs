using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.ViewModels
{
    public class BaseListViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        public BaseListViewModel()
        {
            // Set the initial visibility
            IsListVisible = true;
            IsCreating = false;
            IsEditing = false;
        }

        #region Properties & Fields

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

        private int _pageSize = 10;
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

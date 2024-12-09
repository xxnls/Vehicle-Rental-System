using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.ViewModels
{
    public class BaseListViewModel : BaseViewModel, IDataErrorInfo
    {
        public BaseListViewModel()
        {
            // Set the initial visibility
            IsListVisible = true;
            IsCreating = false;
            IsEditing = false;
        }

        #region Properties & Fields

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

        #region Validation

        // Implement the IDataErrorInfo interface for property validation
        public string Error => string.Join(Environment.NewLine, GetAllErrors().Select(e => e.ErrorMessage));

        public string this[string propertyName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(this) { MemberName = propertyName };

                // Validate the specific property
                Validator.TryValidateProperty(
                    this.GetType().GetProperty(propertyName)?.GetValue(this),
                    context,
                    validationResults
                );

                // Return the first validation error if any, else null
                return validationResults.FirstOrDefault()?.ErrorMessage;
            }
        }

        private IEnumerable<ValidationResult> GetAllErrors()
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(this);

            // Validate all properties of the model
            Validator.TryValidateObject(this, context, validationResults, true);

            return validationResults;
        }

        #endregion
    }
}

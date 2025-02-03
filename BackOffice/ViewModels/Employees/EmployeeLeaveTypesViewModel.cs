using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Employees;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.ViewModels.Employees
{
    public class EmployeeLeaveTypesViewModel : BaseListViewModel<EmployeeLeaveTypeDto>, IListViewModel
    {
        public EmployeeLeaveTypesViewModel() : base("EmployeeLeaveTypes", LocalizationHelper.GetString("EmployeeLeaveTypes", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.EmployeeLeaveTypeId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(() => DeleteModelAsync(EditableModel.EmployeeLeaveTypeId));

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.Description), ValidateDescription },
                { nameof(EditableModel.DefaultDays) , ValidateDefaultDays }
            };
        }

        #region Validation

        // Validation method for Name
        private void ValidateName()
        {
            ClearErrors(nameof(EditableModel.Name));

            if (string.IsNullOrWhiteSpace(EditableModel.Name))
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("Generic", "ErrorName1"));
            }
            else if (EditableModel.Name.Length < 3)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("Generic", "ErrorName2"));
            }
            else if (EditableModel.Name.Length > 100)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("EmployeeLeaveTypes", "ErrorName3"));
            }
        }

        // Validation method for Description
        private void ValidateDescription()
        {
            ClearErrors(nameof(EditableModel.Description));

            if (!string.IsNullOrWhiteSpace(EditableModel.Description) && EditableModel.Description.Length > 300)
            {
                AddError(nameof(EditableModel.Description), LocalizationHelper.GetString("EmployeeLeaveTypes", "ErrorDescription1"));
            }
        }

        // Validation method for DefaultDays
        private void ValidateDefaultDays()
        {
            ClearErrors(nameof(EditableModel.DefaultDays));

            if (EditableModel.DefaultDays <= 0)
            {
                AddError(nameof(EditableModel.DefaultDays), LocalizationHelper.GetString("EmployeeLeaveTypes", "ErrorDefaultDays1"));
            }
        }

        #endregion
    }
}

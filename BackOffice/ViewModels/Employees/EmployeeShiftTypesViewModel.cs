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
    public class EmployeeShiftTypesViewModel : BaseListViewModel<EmployeeShiftTypeDto>, IListViewModel
    {
        public EmployeeShiftTypesViewModel() : base("EmployeeShiftTypes", LocalizationHelper.GetString("EmployeeShiftTypes", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.EmployeeShiftTypeId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.EmployeeShiftTypeId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                //{ nameof(EditableModel.Name), ValidateName },
                //{ nameof(EditableModel.TimeStart), ValidateTimeStart },
                //{ nameof(EditableModel.TimeEnd), ValidateTimeEnd }
            };
        }

        #region Validation
        private void ValidateName()
        {
            ValidateProperty(nameof(EditableModel.Name),
                () => !string.IsNullOrWhiteSpace(EditableModel.Name),
                LocalizationHelper.GetString("EmployeeShiftTypes", "ErrorName"));
        }

        private void ValidateTimeStart()
        {
            ValidateProperty(nameof(EditableModel.TimeStart),
                () => EditableModel.TimeStart < EditableModel.TimeEnd,
                LocalizationHelper.GetString("EmployeeShiftTypes", "ErrorTimeStart"));
        }

        private void ValidateTimeEnd()
        {
            ValidateProperty(nameof(EditableModel.TimeEnd),
                () => EditableModel.TimeEnd > EditableModel.TimeStart,
                LocalizationHelper.GetString("EmployeeShiftTypes", "ErrorTimeEnd"));
        }
        #endregion
    }
}

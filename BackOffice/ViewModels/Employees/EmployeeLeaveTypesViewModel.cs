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
                // TODO: Define validation rules for EmployeeLeaveTypesView
            };
        }
    }
}

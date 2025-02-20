using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Helpers;
using BackOffice.Models;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.Employees;
using BackOffice.ViewModels.Employees;
using BackOffice.Views.Employees;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Other
{
    public class RolesAssignmentViewModel : BaseListViewModel<EmployeeDto>
    {
        public SelectorDialogParameters SelectRoleParameters { get; set; }

        private string _selectedRole = string.Empty;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                ValidateRole();  // Run validation
                (AssignRoleCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
                (RemoveRoleCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand AssignRoleCommand { get; }
        public ICommand RemoveRoleCommand { get; }

        public RolesAssignmentViewModel() : base("Employees",
            LocalizationHelper.GetString("RolesAssignment", "DisplayName"))
        {
            SelectRoleParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(EmployeeRolesViewModel),
                SelectorView = new EmployeeRolesView(),
                TargetProperty = result =>
                {
                    var role = (EmployeeRoleDto)result;
                    SelectedRole = role.Name;
                },
                Title = LocalizationHelper.GetString("RolesAssignment", "SelectRoleTitle")
            };

            SwitchToCreateModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Create), () => false);

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(SelectedRole), ValidateRole }
            };

            AssignRoleCommand = new AsyncRelayCommand(AssignRoleAsync, () => !HasErrors);
            RemoveRoleCommand = new AsyncRelayCommand(RemoveRoleAsync, () => !HasErrors);

            ValidateRole();
        }

        private async Task AssignRoleAsync()
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null || string.IsNullOrWhiteSpace(SelectedRole))
                    return;

                var url = $"EmployeeRoles/user/{EditableModel.Id}/roles/{SelectedRole}";

                await ApiClient.PutAsync(url);

                UpdateStatus(LocalizationHelper.GetString("RolesAssignment", "USSuccessfulAssignment"));
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("RolesAssignment", "USFailedAssignment"));
            }
            finally
            {
                SwitchViewMode(ViewMode.List);

                IsBusy = false;
            }
        }

        private async Task RemoveRoleAsync()
        {
            try
            {
                IsBusy = true;

                if (EditableModel == null || string.IsNullOrWhiteSpace(SelectedRole))
                    return;

                var url = $"EmployeeRoles/user/{EditableModel.Id}/roles/{SelectedRole}";

                await ApiClient.DeleteAsync(url);
            }
            catch (Exception ex)
            {
                UpdateStatus(LocalizationHelper.GetString("RolesAssignment", "USDeleteError") + $" {ex.Message}");
            }
            finally
            {
                SwitchViewMode(ViewMode.List);

                IsBusy = false;
            }
        }

        private void ValidateRole()
        {
            ClearErrors(nameof(SelectedRole));

            if(string.IsNullOrWhiteSpace(SelectedRole))
            {
                AddError(nameof(SelectedRole), LocalizationHelper.GetString("RolesAssignment", "ErrorRole"));
            }
        }

    }
}

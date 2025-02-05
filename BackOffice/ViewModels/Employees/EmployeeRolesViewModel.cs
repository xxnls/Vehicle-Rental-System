using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Employees;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.ViewModels.Employees
{
    public class EmployeeRolesViewModel : BaseListViewModel<EmployeeRoleDto>, IListViewModel
    {
        public EmployeeRolesViewModel()
            : base("EmployeeRoles", LocalizationHelper.GetString("EmployeeRoles", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.Id, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.Id),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Name), ValidateName },
                { nameof(EditableModel.RolePower), ValidateRolePower },
                //{ nameof(EditableModel.ManageVehicles), ValidateManageVehicles },
                //{ nameof(EditableModel.ManageEmployees), ValidateManageEmployees },
                //{ nameof(EditableModel.ManageRentals), ValidateManageRentals },
                //{ nameof(EditableModel.ManageLeaves), ValidateManageLeaves },
                //{ nameof(EditableModel.ManageSchedule), ValidateManageSchedule }
            };
        }

        #region Validation

        // Validation method for Name
        private void ValidateName()
        {
            ClearErrors(nameof(EditableModel.Name));

            if (string.IsNullOrWhiteSpace(EditableModel.Name))
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("EmployeeRoles", "ErrorName1"));
            }
            else if (EditableModel.Name.Length < 3)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("EmployeeRoles", "ErrorName2"));
            }
            else if (EditableModel.Name.Length >= 50)
            {
                AddError(nameof(EditableModel.Name), LocalizationHelper.GetString("EmployeeRoles", "ErrorName3"));
            }
        }

        // Validation method for RolePower
        private void ValidateRolePower()
        {
            ClearErrors(nameof(EditableModel.RolePower));

            if (EditableModel.RolePower <= 0)
            {
                AddError(nameof(EditableModel.RolePower), LocalizationHelper.GetString("EmployeeRoles", "ErrorRolePower1"));
            }
            else if (EditableModel.RolePower > 1000)
            {
                AddError(nameof(EditableModel.RolePower), LocalizationHelper.GetString("EmployeeRoles", "ErrorRolePower2"));
            }
        }

        //// Validation method for ManageVehicles
        //private void ValidateManageVehicles()
        //{
        //    ClearErrors(nameof(EditableModel.ManageVehicles));

        //    if (EditableModel.ManageVehicles == null)
        //    {
        //        AddError(nameof(EditableModel.ManageVehicles), LocalizationHelper.GetString("EmployeeRoles", "ErrorPermission1"));
        //    }
        //}

        //// Validation method for ManageEmployees
        //private void ValidateManageEmployees()
        //{
        //    ClearErrors(nameof(EditableModel.ManageEmployees));
        //}

        //// Validation method for ManageRentals
        //private void ValidateManageRentals()
        //{
        //    ClearErrors(nameof(EditableModel.ManageRentals));
        //}

        //// Validation method for ManageLeaves
        //private void ValidateManageLeaves()
        //{
        //    ClearErrors(nameof(EditableModel.ManageLeaves));
        //}

        //// Validation method for ManageSchedule
        //private void ValidateManageSchedule()
        //{
        //    ClearErrors(nameof(EditableModel.ManageSchedule));
        //}

        #endregion
    }
}
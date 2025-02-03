using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Employees;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace BackOffice.ViewModels.Employees
{
    public class EmployeePositionsViewModel : BaseListViewModel<EmployeePositionDto>, IListViewModel
    {
        private bool _isSalaryDisabled;
        private bool _isHourlyRateDisabled;

        public bool IsSalaryDisabled
        {
            get => _isSalaryDisabled;
            set
            {
                if (_isSalaryDisabled != value)
                {
                    _isSalaryDisabled = value;
                    OnPropertyChanged();
                    if (_isSalaryDisabled)
                    {
                        EditableModel.DefaultBaseSalary = null;
                    }
                }
            }
        }

        public bool IsHourlyRateDisabled
        {
            get => _isHourlyRateDisabled;
            set
            {
                if (_isHourlyRateDisabled != value)
                {
                    _isHourlyRateDisabled = value;
                    OnPropertyChanged();
                    if (_isHourlyRateDisabled)
                    {
                        EditableModel.DefaultHourlyRate = null;
                    }
                }
            }
        }

        public EmployeePositionsViewModel() : base("EmployeePositions", LocalizationHelper.GetString("EmployeePositions", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(() => CreateModelAsync(EditableModel));
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.EmployeePositionId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.EmployeePositionId),
                () => EditableModel != null
            );

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.Title), ValidateTitle },
                //{ nameof(EditableModel.DefaultBaseSalary), ValidateDefaultBaseSalary },
                //{ nameof(EditableModel.DefaultHourlyRate), ValidateDefaultHourlyRate }
            };
        }

        #region Validation
        private void ValidateTitle()
        {
            ClearErrors(nameof(EditableModel.Title));

            if (string.IsNullOrWhiteSpace(EditableModel.Title))
            {
                AddError(nameof(EditableModel.Title), LocalizationHelper.GetString("EmployeePositions", "ErrorTitle1"));
            }
            else if (EditableModel.Title.Length > 100)
            {
                AddError(nameof(EditableModel.Title), LocalizationHelper.GetString("EmployeePositions", "ErrorTitle2"));
            }
        }

        //private void ValidateDefaultBaseSalary()
        //{
        //    ValidateProperty(nameof(EditableModel.DefaultBaseSalary),
        //        () => EditableModel.DefaultBaseSalary >= 0,
        //        LocalizationHelper.GetString("EmployeePositions", "ErrorDefaultBaseSalary1"));
        //}

        //private void ValidateDefaultHourlyRate()
        //{
        //    ValidateProperty(nameof(EditableModel.DefaultHourlyRate),
        //        () => EditableModel.DefaultHourlyRate >= 0,
        //        LocalizationHelper.GetString("EmployeePositions", "ErrorDefaultHourlyRate1"));
        //}
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models;
using BackOffice.Models.DTOs.Other;
using BackOffice.Views.Other;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Other
{
    public class AddressesViewModel : BaseListViewModel<AddressDto>, IListViewModel
    {
        public SelectorDialogParameters SelectCountryParameters { get; set; }

        public AddressesViewModel() : base("Addresses", LocalizationHelper.GetString("Addresses", "DisplayName"))
        {
            SelectCountryParameters = new SelectorDialogParameters
            {
                SelectorViewModelType = typeof(CountriesViewModel),
                SelectorView = new CountriesView(),
                TargetProperty = result =>
                {
                    EditableModel.Country = (CountryDto)result;
                },
                Title = LocalizationHelper.GetString("Addresses", "SelectCountryTitle")
            };

            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(() => UpdateModelAsync(EditableModel.AddressId, EditableModel));
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.AddressId),
                () => false
            );

            // Block the command for switching to create mode and for restoring the model
            SwitchToCreateModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Create), () => false);
            RestoreModelCommand = new AsyncRelayCommand<int>(id => RestoreModelAsync(id, EditableModel), id => false);

            ValidationRules = new Dictionary<string, Action>
            {
                { nameof(EditableModel.FirstLine), ValidateFirstLine },
                { nameof(EditableModel.SecondLine), ValidateSecondLine },
                { nameof(EditableModel.ZipCode), ValidateZipCode },
                { nameof(EditableModel.City), ValidateCity }
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            //N/A
        }

        #endregion

        #region Validation

        // Validation method for FirstLine
        private void ValidateFirstLine()
        {
            ClearErrors(nameof(EditableModel.FirstLine));

            if (string.IsNullOrWhiteSpace(EditableModel.FirstLine))
            {
                AddError(nameof(EditableModel.FirstLine), LocalizationHelper.GetString("Addresses", "ErrorFirstLine1"));
            }
            else if (EditableModel.FirstLine.Length > 100)
            {
                AddError(nameof(EditableModel.FirstLine), LocalizationHelper.GetString("Addresses", "ErrorFirstLine2"));
            }
        }

        // Validation method for SecondLine
        private void ValidateSecondLine()
        {
            ClearErrors(nameof(EditableModel.SecondLine));

            if (!string.IsNullOrWhiteSpace(EditableModel.SecondLine) && EditableModel.SecondLine.Length > 100)
            {
                AddError(nameof(EditableModel.SecondLine), LocalizationHelper.GetString("Addresses", "ErrorSecondLine1"));
            }
        }

        // Validation method for ZipCode
        private void ValidateZipCode()
        {
            ClearErrors(nameof(EditableModel.ZipCode));

            if (string.IsNullOrWhiteSpace(EditableModel.ZipCode))
            {
                AddError(nameof(EditableModel.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode1"));
            }
            else if (EditableModel.ZipCode.Length > 20)
            {
                AddError(nameof(EditableModel.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode2"));
            }
        }

        // Validation method for City
        private void ValidateCity()
        {
            ClearErrors(nameof(EditableModel.City));

            if (string.IsNullOrWhiteSpace(EditableModel.City))
            {
                AddError(nameof(EditableModel.City), LocalizationHelper.GetString("Addresses", "ErrorCity1"));
            }
            else if (EditableModel.City.Length > 50)
            {
                AddError(nameof(EditableModel.City), LocalizationHelper.GetString("Addresses", "ErrorCity2"));
            }
        }

        #endregion
    }
}

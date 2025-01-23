using System;
using System.Collections.Generic;
using System.Linq;
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
                // TODO: Fill it when updating is implemented
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {
            //N/A
        }

        #endregion
    }
}

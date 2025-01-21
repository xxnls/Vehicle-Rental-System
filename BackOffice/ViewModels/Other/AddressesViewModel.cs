using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Other;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Other
{
    public class AddressesViewModel : BaseListViewModel<AddressDto>, IListViewModel
    {
        public AddressesViewModel() : base("Addresses", LocalizationHelper.GetString("Addresses", "DisplayName"))
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.AddressId),
                () => false
            );
            SwitchToCreateModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Create), () => false);
            SwitchToEditModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Edit), () => false);

            ValidationRules = new Dictionary<string, Action>
            {
                // TODO: Fill it when updating is implemented
            };
        }

        #region Methods

        public async Task CreateModelAsync()
        {

        }

        public async Task UpdateModelAsync()
        {
            // TODO: Implement it
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Other;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels.Other
{
    public class CountriesViewModel : BaseListViewModel<CountryDto>, IListViewModel
    {
        public CountriesViewModel() : base("Countries", LocalizationHelper.GetString("Countries", "DisplayName"))
        {
            // CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            // UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);

            // Block usage of switching to edit/create view, delete, restore, show deleted, filter options
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.CountryId),
                () => false
            );
            SwitchToCreateModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Create), () => false);
            SwitchToEditModeCommand = new RelayCommand(() => SwitchViewMode(ViewMode.Edit), () => false);
            RestoreModelCommand = new AsyncRelayCommand<int>(id => RestoreModelAsync(id, EditableModel), id => false);
            ShowDeletedModelsCommand = new RelayCommand(ShowDeletedModels, () => false);
            ShowFilterOptionsCommand = new RelayCommand(ShowFilterOptions, () => false);
        }
    }
}

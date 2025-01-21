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
    public class CountriesViewModel : BaseListViewModel<CountryDto>, IListViewModel
    {
        public CountriesViewModel() : base("Countries", "CHANGE THIS NAME IN COUNTRIES")
        {
            CreateModelCommand = new AsyncRelayCommand(CreateModelAsync);
            UpdateModelCommand = new AsyncRelayCommand(UpdateModelAsync);
            DeleteModelCommand = new AsyncRelayCommand(
                () => DeleteModelAsync(EditableModel.CountryId),
                () => EditableModel != null
            );
        }

        #region Methods

        public async Task CreateModelAsync()
        {

        }

        public async Task UpdateModelAsync()
        {
            
        }

        #endregion
    }
}

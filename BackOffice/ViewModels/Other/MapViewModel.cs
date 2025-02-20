using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Other;
using BackOffice.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Tiling;
using Mapsui.UI.Wpf;


namespace BackOffice.ViewModels.Other
{
    public class MapViewModel : BaseListViewModel<LocationDto>, IBaseListViewModel
    {
        public MapViewModel() : base("Locations", LocalizationHelper.GetString("MainWindow", "MapDisplayName"))
        {

        }
    }
}
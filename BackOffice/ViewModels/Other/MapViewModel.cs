using BackOffice.Helpers;
using BackOffice.Interfaces;
using BackOffice.Models.DTOs.Other;
using BackOffice.Services;


namespace BackOffice.ViewModels.Other
{
    public class MapViewModel : BaseListViewModel<LocationDto>, IBaseListViewModel
    {
        public MapViewModel() : base("Locations", LocalizationHelper.GetString("MainWindow", "MapDisplayName"))
        {
        }
    }
}
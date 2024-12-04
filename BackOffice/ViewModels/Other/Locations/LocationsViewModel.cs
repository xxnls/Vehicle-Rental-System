using BackOffice.Services;
using BackOffice.Services.Other;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Models.Other.Locations;

namespace BackOffice.ViewModels.Other.Locations
{
    public class LocationsViewModel : BaseViewModel
    {
        private readonly LocationServices _locationServices;

        public ObservableCollection<Location> Locations { get; }

        public ICommand LoadLocationsCommand { get; private set; }

        public LocationsViewModel()
        {
            _locationServices = new LocationServices();
            Locations = new ObservableCollection<Location>();
            LoadLocationsCommand = new RelayCommand(async () => await LoadLocationsAsync());

            // Load locations as soon as view model is created
            LoadLocationsAsync();
        }

        private async Task LoadLocationsAsync()
        {
            var locations = await _locationServices.GetLocationsAsync();
            Locations.Clear();
            foreach (var location in locations)
            {
                Locations.Add(location);
            }
            UpdateStatus("Locations loaded successfully.");
        }
    }
}

using BackOffice.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BackOffice.Models.Vehicles;
using BackOffice.Services.Vehicles;
using CommunityToolkit.Mvvm.Input;

namespace BackOffice.ViewModels
{
    public class VehiclesViewModel : BaseViewModel
    {
        private readonly VehiclesService _vehiclesService;

        public ObservableCollection<Vehicle> Vehicles { get; set; } = new();

        public ICommand LoadVehiclesCommand { get; }
        public ICommand AddVehicleCommand { get; }

        public VehiclesViewModel()
        {
            _vehiclesService = new VehiclesService();
            LoadVehiclesCommand = new RelayCommand(async () => await LoadVehiclesAsync());
            //AddVehicleCommand = new RelayCommand(async () => await AddVehicleAsync());
        }

        private async Task LoadVehiclesAsync()
        {
            var vehicles = await _vehiclesService.GetVehiclesAsync();
            Vehicles.Clear();
            foreach (var vehicle in vehicles)
            {
                Vehicles.Add(vehicle);
            }
        }

        //private async Task AddVehicleAsync()
        //{
        //    var newVehicle = new CreateVehicleDTO
        //    {
        //        Name = "New Car",
        //        Type = "SUV",
        //        IsAvailable = true
        //    };

        //    var createdVehicle = await _vehiclesService.CreateVehicleAsync(newVehicle);
        //    Vehicles.Add(createdVehicle);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.Vehicles;

namespace BackOffice.Services.Vehicles
{
    public class VehiclesService
    {
        private readonly ApiClient _apiClient;

        public VehiclesService()
        {
            _apiClient = new ApiClient();
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            return await _apiClient.GetAsync<List<Vehicle>>("vehicles");
        }

        //public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDTO newVehicle)
        //{
        //    return await _apiClient.PostAsync<CreateVehicleDTO, Vehicle>("vehicles", newVehicle);
        //}

        //public async Task UpdateVehicleAsync(int id, UpdateVehicleDTO updatedVehicle)
        //{
        //    await _apiClient.PutAsync($"vehicles/{id}", updatedVehicle);
        //}

        //public async Task DeleteVehicleAsync(int id)
        //{
        //    await _apiClient.DeleteAsync($"vehicles/{id}");
        //}
    }
}

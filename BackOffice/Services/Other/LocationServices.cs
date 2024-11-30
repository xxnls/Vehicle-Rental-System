using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.Other.Locations;

namespace BackOffice.Services.Other
{
    public class LocationServices
    {
        private readonly ApiClient _apiClient;

        public LocationServices()
        {
            _apiClient = new ApiClient();
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            return await _apiClient.GetAsync<List<Location>>("locations");
        }
    }
}

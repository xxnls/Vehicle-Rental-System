using API.Interfaces;

namespace API.Models.DTOs.Other
{
    public class LocationDto : ILocation
    {
        public int LocationId { get; set; }

        public int? VehicleId { get; set; }

        public int? RentalPlaceId { get; set; }

        public double Gpslatitude { get; set; }

        public double Gpslongitude { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsActive { get; set; }
    }
}

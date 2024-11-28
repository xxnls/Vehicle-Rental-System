namespace API.Models.Other.Locations.DTOs
{
    public class CreateLocationDTO
    {
        // Required details for the location
        public double Gpslatitude { get; set; }  // Latitude coordinate for the location
        public double Gpslongitude { get; set; } // Longitude coordinate for the location
        public DateTime DateTime { get; set; }   // The timestamp when the location was recorded

        // Optional foreign keys
        public int? VehicleId { get; set; }      // Optional, ID of the vehicle linked to this location
        public int? RentalPlaceId { get; set; }  // Optional, ID of the rental place associated with the location
    }
}

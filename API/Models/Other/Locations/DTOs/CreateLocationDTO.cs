namespace API.Models.Other.Locations.DTOs
{
    public class CreateLocationDTO
    {
        // Required details for the location
        public double Gpslatitude { get; set; }  
        public double Gpslongitude { get; set; } 
        public DateTime DateTime { get; set; }   

        // Optional foreign keys
        public int? VehicleId { get; set; }      
        public int? RentalPlaceId { get; set; } 
    }
}

namespace API.Models.DTOs.Vehicles
{
    public class VehicleOptionalInformationDto
    {
        public int VehicleOptionalInformationId { get; set; }

        public bool HasNavigation { get; set; }

        public bool HasBluetooth { get; set; }

        public bool HasAirConditioning { get; set; }

        public bool HasAutomaticTransmission { get; set; }

        public bool HasParkingSensors { get; set; }

        public bool HasCruiseControl { get; set; }
    }
}

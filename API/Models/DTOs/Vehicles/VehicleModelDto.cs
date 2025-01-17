namespace API.Models.DTOs.Vehicles
{
    public class VehicleModelDto
    {
        public int VehicleModelId { get; set; }

        public int VehicleBrandId { get; set; }

        public string Name { get; set; } = null!;

        public double? EngineSize { get; set; }

        public int? HorsePower { get; set; }

        public string FuelType { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }
    }
}

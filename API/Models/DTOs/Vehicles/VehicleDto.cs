using API.Interfaces;

namespace API.Models.DTOs.Vehicles
{
    public class VehicleDto : IBaseModel
    {
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleModelId { get; set; }
        public int VehicleStatisticsId { get; set; }
        public int VehicleOptionalInformationId { get; set; }
        public int RentalPlaceId { get; set; }
        public int LocationId { get; set; }
        public string Vin { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int ManufactureYear { get; set; }
        public int CurrentMileage { get; set; }
        public int? LastMaintenanceMileage { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? CustomDailyRate { get; set; }
        public decimal? CustomWeeklyRate { get; set; }
        public decimal? CustomDeposit { get; set; }
        public bool IsAvailableForRent { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public VehicleTypeDto? VehicleType { get; set; }
        public VehicleModelDto? VehicleModel { get; set; }
        public VehicleStatisticsDto? VehicleStatistics { get; set; }
        public VehicleOptionalInformationDto? OptionalInformation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using API.Interfaces;

namespace API.Models.Vehicles;

public partial class Vehicle : IBaseModel
{
    public int VehicleId { get; set; }

    public int VehicleModelId { get; set; }

    public int VehicleTypeId { get; set; }

    public int VehicleStatisticsId { get; set; }

    public int VehicleOptionalInformationId { get; set; }

    public int RentalPlaceId { get; set; }

    public int LocationId { get; set; }

    public string? Vin { get; set; }

    public string? LicensePlate { get; set; }

    public string Color { get; set; } = null!;

    public int ManufactureYear { get; set; }

    public int CurrentMileage { get; set; }

    public int? LastMaintenanceMileage { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }

    public DateTime? NextMaintenanceDate { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal PurchasePrice { get; set; }

    public string Status { get; set; } = null!;

    public decimal? CustomDailyRate { get; set; }

    public decimal? CustomWeeklyRate { get; set; }

    public decimal? CustomDeposit { get; set; }

    public bool IsAvailableForRent { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Location Location { get; set; } = null!;

    public virtual RentalPlace RentalPlace { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual VehicleModel VehicleModel { get; set; } = null!;

    public virtual VehicleOptionalInformation VehicleOptionalInformation { get; set; } = null!;

    public virtual VehicleStatistic VehicleStatistics { get; set; } = null!;

    public virtual VehicleType VehicleType { get; set; } = null!;
}

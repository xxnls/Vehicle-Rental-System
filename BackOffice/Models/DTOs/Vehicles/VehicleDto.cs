using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleDto : BaseDtoModel
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

        // Navigation properties
        private VehicleTypeDto? _vehicleType;
        public VehicleTypeDto? VehicleType
        {
            get => _vehicleType;
            set
            {
                _vehicleType = value;
                OnPropertyChanged();
            }
        }

        private VehicleModelDto? _vehicleModel;
        public VehicleModelDto? VehicleModel
        {
            get => _vehicleModel;
            set
            {
                _vehicleModel = value;
                OnPropertyChanged();
            }
        }

        private RentalPlaceDto? _rentalPlace;
        public RentalPlaceDto? RentalPlace
        {
            get => _rentalPlace;
            set
            {
                _rentalPlace = value;
                OnPropertyChanged();
            }
        }

        private VehicleStatisticsDto? _vehicleStatistics;
        public VehicleStatisticsDto? VehicleStatistics
        {
            get => _vehicleStatistics;
            set
            {
                _vehicleStatistics = value;
                OnPropertyChanged();
            }
        }

        private VehicleOptionalInformationDto? _optionalInformation;
        public VehicleOptionalInformationDto? OptionalInformation
        {
            get => _optionalInformation;
            set
            {
                _optionalInformation = value;
                OnPropertyChanged();
            }
        }
    }
}

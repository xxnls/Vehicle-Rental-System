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
        private int _vehicleTypeId;
        public int VehicleTypeId
        {
            get => _vehicleTypeId;
            set
            {
                _vehicleTypeId = value;
                OnPropertyChanged();
            }
        }

        private int _vehicleModelId;
        public int VehicleModelId
        {
            get => _vehicleModelId;
            set
            {
                _vehicleModelId = value;
                OnPropertyChanged();
            }
        }

        private int? _vehicleStatisticsId;
        public int? VehicleStatisticsId
        {
            get => _vehicleStatisticsId;
            set
            {
                _vehicleStatisticsId = value;
                OnPropertyChanged();
            }
        }

        private int? _vehicleOptionalInformationId;
        public int? VehicleOptionalInformationId
        {
            get => _vehicleOptionalInformationId;
            set
            {
                _vehicleOptionalInformationId = value;
                OnPropertyChanged();
            }
        }

        private int? _rentalPlaceId;
        public int? RentalPlaceId
        {
            get => _rentalPlaceId;
            set
            {
                _rentalPlaceId = value;
                OnPropertyChanged();
            }
        }

        private string _vin = string.Empty;
        public string Vin
        {
            get => _vin;
            set
            {
                _vin = value;
                OnPropertyChanged();
            }
        }

        private string _licensePlate = string.Empty;
        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                _licensePlate = value;
                OnPropertyChanged();
            }
        }

        private string _color = string.Empty;
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        private int _manufactureYear;
        public int ManufactureYear
        {
            get => _manufactureYear;
            set
            {
                _manufactureYear = value;
                OnPropertyChanged();
            }
        }

        private int _currentMileage;
        public int CurrentMileage
        {
            get => _currentMileage;
            set
            {
                _currentMileage = value;
                OnPropertyChanged();
            }
        }

        private int? _lastMaintenanceMileage;
        public int? LastMaintenanceMileage
        {
            get => _lastMaintenanceMileage;
            set
            {
                _lastMaintenanceMileage = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _lastMaintenanceDate;
        public DateTime? LastMaintenanceDate
        {
            get => _lastMaintenanceDate;
            set
            {
                _lastMaintenanceDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _nextMaintenanceDate;
        public DateTime? NextMaintenanceDate
        {
            get => _nextMaintenanceDate;
            set
            {
                _nextMaintenanceDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _purchaseDate;
        public DateTime? PurchaseDate
        {
            get => _purchaseDate;
            set
            {
                _purchaseDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _purchasePrice;
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged();
            }
        }

        private string _status = string.Empty;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private decimal? _customDailyRate;
        public decimal? CustomDailyRate
        {
            get => _customDailyRate;
            set
            {
                _customDailyRate = value;
                OnPropertyChanged();
            }
        }

        private decimal? _customWeeklyRate;
        public decimal? CustomWeeklyRate
        {
            get => _customWeeklyRate;
            set
            {
                _customWeeklyRate = value;
                OnPropertyChanged();
            }
        }

        private decimal? _customDeposit;
        public decimal? CustomDeposit
        {
            get => _customDeposit;
            set
            {
                _customDeposit = value;
                OnPropertyChanged();
            }
        }

        private bool _isAvailableForRent;
        public bool IsAvailableForRent
        {
            get => _isAvailableForRent;
            set
            {
                _isAvailableForRent = value;
                OnPropertyChanged();
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

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

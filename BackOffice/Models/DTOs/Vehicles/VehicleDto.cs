﻿using System;
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
        public int VehicleStatusId { get; set; }
        public int VehicleStatisticsId { get; set; }
        public int VehicleOptionalInformationId { get; set; }
        public int RentalPlaceId { get; set; }
        public int LocationId { get; set; }

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

        private int? _manufactureYear;
        public int? ManufactureYear
        {
            get => _manufactureYear;
            set
            {
                _manufactureYear = value;
                OnPropertyChanged();
            }
        }

        private int? _currentMileage;
        public int? CurrentMileage
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

        private decimal? _purchasePrice;
        public decimal? PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
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

        private VehicleStatusDto? _vehicleStatus;
        public VehicleStatusDto? VehicleStatus
        {
            get => _vehicleStatus;
            set
            {
                _vehicleStatus = value;
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

        private VehicleOptionalInformationDto? _vehicleOptionalInformation = new();
        public VehicleOptionalInformationDto? VehicleOptionalInformation
        {
            get => _vehicleOptionalInformation;
            set
            {
                _vehicleOptionalInformation = value;
                OnPropertyChanged();
            }
        }

        private LocationDto? _location;
        public LocationDto? Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }
    }
}

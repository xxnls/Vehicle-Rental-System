using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class RentalPlaceDto : BaseDtoModel
    {
        public int RentalPlaceId { get; set; }

        private int _locationId;
        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged();
                }
            }
        }

        private double? _gpsLatitude;
        public double? GpsLatitude
        {
            get => _gpsLatitude;
            set
            {
                if (_gpsLatitude != value)
                {
                    _gpsLatitude = value;
                    OnPropertyChanged();
                }
            }
        }

        private double? _gpsLongitude;
        public double? GpsLongitude
        {
            get => _gpsLongitude;
            set
            {
                if (_gpsLongitude != value)
                {
                    _gpsLongitude = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _addressId;
        public int AddressId
        {
            get => _addressId;
            set
            {
                if (_addressId != value)
                {
                    _addressId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _city;
        public string? City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _firstLine;
        public string? FirstLine
        {
            get => _firstLine;
            set
            {
                if (_firstLine != value)
                {
                    _firstLine = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _secondLine;
        public string? SecondLine
        {
            get => _secondLine;
            set
            {
                if (_secondLine != value)
                {
                    _secondLine = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}

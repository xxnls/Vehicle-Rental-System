using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class LocationDto : BaseDtoModel
    {
        public int LocationId { get; set; }

        public int? VehicleId { get; set; }

        public int? RentalPlaceId { get; set; }

        private double _gpsLatitude;
        public double GpsLatitude
        {
            get => _gpsLatitude;
            set
            {
                _gpsLatitude = value;
                OnPropertyChanged();
            }
        }

        private double _gpsLongitude;
        public double GpsLongitude
        {
            get => _gpsLongitude;
            set
            {
                _gpsLongitude = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateTime { get; set; }
    }
}

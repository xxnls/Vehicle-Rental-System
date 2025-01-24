using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class LocationDto
    {
        public int LocationId { get; set; }

        public int? VehicleId { get; set; }

        public int? RentalPlaceId { get; set; }

        public double GpsLatitude { get; set; }

        public double GpsLongitude { get; set; }

        public DateTime DateTime { get; set; }

        public bool IsActive { get; set; }
    }
}

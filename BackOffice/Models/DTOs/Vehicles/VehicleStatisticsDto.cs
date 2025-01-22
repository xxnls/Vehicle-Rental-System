using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleStatisticsDto
    {
        public int VehicleStatisticsId { get; set; }

        public int TotalRentals { get; set; }

        public decimal RentalRevenue { get; set; }

        public DateTime? FirstRentalDate { get; set; }

        public DateTime? LastRentalDate { get; set; }
    }
}

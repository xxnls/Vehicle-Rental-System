﻿using System;
using System.Collections.Generic;

namespace BackOffice.Models.Vehicles;

public partial class VehicleStatistics
{
    public int VehicleStatisticsId { get; set; }

    public int TotalRentals { get; set; }

    public decimal RentalRevenue { get; set; }

    public DateTime? FirstRentalDate { get; set; }

    public DateTime? LastRentalDate { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

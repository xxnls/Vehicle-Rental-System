﻿using System;
using System.Collections.Generic;

namespace BackOffice.Models.Customers;

public partial class CustomerStatistics
{
    public int CustomerStatisticsId { get; set; }

    public int TotalRentals { get; set; }

    public short ActiveRentals { get; set; }

    public int CanceledRentals { get; set; }

    public DateTime? FirstRentalDate { get; set; }

    public DateTime? LastRentalDate { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}

using API.Models.Employees;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostRentalReport
{
    public int PostRentalReportId { get; set; }

    public int InspectorEmployeeId { get; set; }

    public bool IsCustomerLate { get; set; }

    public bool IsCarDamaged { get; set; }

    public bool IsCarRefueled { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Employee InspectorEmployee { get; set; } = null!;

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

using API.Models.Vehicles;
using System;
using System.Collections.Generic;
using API.Interfaces;
using API.Models.Employees;

namespace API.Models.Other;

public partial class RentalPlace : IBaseModel
{
    public int RentalPlaceId { get; set; }

    public int LocationId { get; set; }

    public int AddressId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

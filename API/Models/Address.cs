using API.Models.Other;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public short CountryId { get; set; }

    public string FirstLine { get; set; } = null!;

    public string? SecondLine { get; set; }

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<RentalPlace> RentalPlaces { get; set; } = new List<RentalPlace>();
}

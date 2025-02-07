using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using API.Interfaces;
using API.Models.Rentals;

namespace API.Models.Customers;

public partial class Customer : IdentityUser<int>, IBaseModel
{
    public int AddressId { get; set; }

    public int CustomerTypeId { get; set; }

    public int CustomerStatisticsId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? CompanyName { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual CustomerStatistic CustomerStatistics { get; set; } = null!;

    public virtual CustomerType CustomerType { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

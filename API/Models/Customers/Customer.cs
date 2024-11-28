using API.Models.Documents;
using API.Models.Other.Addresses;
using API.Models.Rentals;
using System;
using System.Collections.Generic;

namespace API.Models.Customers;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int AddressId { get; set; }

    public int CustomerTypeId { get; set; }

    public int CustomerStatisticsId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? CompanyName { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual CustomerStatistics CustomerStatistics { get; set; } = null!;

    public virtual CustomerType CustomerType { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

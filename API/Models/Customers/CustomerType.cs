using System;
using System.Collections.Generic;

namespace API.Models.Customers;

public partial class CustomerType
{
    public int CustomerTypeId { get; set; }

    public string CustomerType1 { get; set; } = null!;

    public short? MaxRentals { get; set; }

    public double? DiscountPercent { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}

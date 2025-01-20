using System;
using System.Collections.Generic;

namespace API.Models.Other;

public partial class Country 
{
    public short CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public string DialingCode { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}

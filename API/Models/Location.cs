using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public int? VehicleId { get; set; }

    public int? RentalPlaceId { get; set; }

    public double Gpslatitude { get; set; }

    public double Gpslongitude { get; set; }

    public DateTime DateTime { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<RentalPlace> RentalPlaces { get; set; } = new List<RentalPlace>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

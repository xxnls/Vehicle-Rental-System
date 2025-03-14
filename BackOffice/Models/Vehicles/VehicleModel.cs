﻿using System;
using System.Collections.Generic;
using BackOffice.Models.Vehicles.VehicleBrands;

namespace BackOffice.Models.Vehicles;

public partial class VehicleModel
{
    public int VehicleModelId { get; set; }

    public int VehicleBrandId { get; set; }

    public string Name { get; set; } = null!;

    public double? EngineSize { get; set; }

    public int? HorsePower { get; set; }

    public string FuelType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual VehicleBrand VehicleBrand { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

﻿using API.Interfaces;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class VehicleBrand : IBaseModel
{
    public int VehicleBrandId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Website { get; set; }

    public string? LogoUrl { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
}

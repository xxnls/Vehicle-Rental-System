using System;
using System.Collections.Generic;
using API.Interfaces;

namespace API.Models.Vehicles;

public partial class VehicleType : IBaseModel
{
    public int VehicleTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal BaseDailyRate { get; set; }

    public decimal BaseWeeklyRate { get; set; }

    public decimal BaseDeposit { get; set; }

    public string RequiredLicenseType { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

﻿using API.Interfaces;

namespace API.Models.DTOs.Vehicles
{
    public class VehicleModelDto : IBaseModel
    {
        public int VehicleModelId { get; set; }

        public string Name { get; set; } = null!;

        public double? EngineSize { get; set; }

        public int? HorsePower { get; set; }

        public string FuelType { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        // Navigation Properties
        public VehicleBrandDto VehicleBrand { get; set; } = null!;
    }
}

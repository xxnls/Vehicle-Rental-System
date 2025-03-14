﻿using API.Interfaces;

namespace API.Models.DTOs.Vehicles
{
    public class VehicleBrandDto : IBaseModel
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
    }
}

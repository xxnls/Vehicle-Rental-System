using API.Interfaces;

namespace API.Models.DTOs.Vehicles
{
    public class VehicleTypeDto : IBaseModel
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
    }
}

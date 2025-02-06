using API.Interfaces;

namespace API.Models.DTOs.Customers
{
    public class CustomerTypeDto : IBaseModel
    {
        public int CustomerTypeId { get; set; }

        public string CustomerType { get; set; } = null!;

        public short? MaxRentals { get; set; }

        public double? DiscountPercent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }
    }
}

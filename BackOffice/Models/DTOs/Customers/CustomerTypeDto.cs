using BackOffice.Interfaces;

namespace BackOffice.Models.DTOs.Customers
{
    public class CustomerTypeDto : BaseDtoModel
    {
        public int CustomerTypeId { get; set; }

        public string CustomerType { get; set; } = null!;

        public short? MaxRentals { get; set; }

        public double? DiscountPercent { get; set; }
    }
}

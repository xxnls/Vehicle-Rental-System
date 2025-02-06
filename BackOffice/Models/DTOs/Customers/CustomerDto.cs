using BackOffice.Interfaces;
using BackOffice.Models.Customers;
using BackOffice.Models.DTOs.Other;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.Models.DTOs.Customers
{
    public class CustomerDto : BaseDtoModel
    {
        public int AddressId { get; set; }

        public int CustomerTypeId { get; set; }

        public int CustomerStatisticsId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? CompanyName { get; set; }


        // Navigation properties
        public AddressDto Address { get; set; } = null!;

        public CustomerStatisticsDto CustomerStatistics { get; set; } = null!;

        public CustomerTypeDto CustomerType { get; set; } = null!;
    }
}

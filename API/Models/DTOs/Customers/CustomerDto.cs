﻿using API.Interfaces;
using API.Models.Customers;
using API.Models.DTOs.Other;

namespace API.Models.DTOs.Customers
{
    public class CustomerDto : IBaseModel
    {
        public int AddressId { get; set; }

        public int CustomerTypeId { get; set; }

        public int CustomerStatisticsId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? CompanyName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        public AddressDto Address { get; set; } = null!;

        public CustomerStatisticsDto CustomerStatistics { get; set; } = null!;

        public CustomerTypeDto CustomerType { get; set; } = null!;
    }
}

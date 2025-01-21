using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class AddressDto
    {
        public int AddressId { get; set; }

        public string? CountryName { get; set; } = null!;

        public short CountryId { get; set; }

        public string FirstLine { get; set; } = null!;

        public string? SecondLine { get; set; }

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class CountryDto : BaseDtoModel
    {
        public short CountryId { get; set; }

        public string Name { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Abbreviation { get; set; } = null!;

        public string DialingCode { get; set; } = null!;

        public new DateTime? CreatedDate { get; set; }

        public new bool? IsActive { get; set; }
    }
}

namespace API.Models.DTOs.Other
{
    public class CountryDto
    {
        public short CountryId { get; set; }

        public string Name { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Abbreviation { get; set; } = null!;

        public string DialingCode { get; set; } = null!;
    }
}

using API.Models.DTOs.Employees;

namespace API.Models.DTOs.Rentals
{
    public class PostRentalReportDto
    {
        public int PostRentalReportId { get; set; }

        public int InspectorEmployeeId { get; set; }

        public bool IsCustomerLate { get; set; }

        public bool IsCarDamaged { get; set; }

        public bool IsCarRefueled { get; set; }

        public DateTime CreatedDate { get; set; }

        public EmployeeDto InspectorEmployee { get; set; } = null!;
    }
}
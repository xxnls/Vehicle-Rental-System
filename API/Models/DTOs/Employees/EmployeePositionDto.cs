using API.Interfaces;

namespace API.Models.DTOs.Employees
{
    public class EmployeePositionDto : IBaseModel
    {
        public int EmployeePositionId { get; set; }

        public string Title { get; set; } = null!;

        public decimal? DefaultBaseSalary { get; set; }

        public decimal? DefaultHourlyRate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }
    }
}

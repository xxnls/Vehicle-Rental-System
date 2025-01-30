using BackOffice.Models;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeShiftTypeDto : BaseDtoModel
    {
        public int EmployeeShiftTypeId { get; set; }

        public string? Name { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }
    }
}

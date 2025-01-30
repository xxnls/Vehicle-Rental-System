using API.Interfaces;

namespace API.Models.DTOs.Employees
{
    public class EmployeeShiftTypeDto : IBaseModel
    {
        public int EmployeeShiftTypeId { get; set; }

        public string? Name { get; set; }

        public DateTime TimeStart { get; set; }

        public DateTime TimeEnd { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }
    }
}

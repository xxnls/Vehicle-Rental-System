using API.Interfaces;
using API.Models.DTOs.Other;

namespace API.Models.DTOs.Employees
{
    public class EmployeeScheduleDto : IBaseModel
    {
        public int EmployeeScheduleId { get; set; }

        public int EmployeeShiftTypeId { get; set; }

        public int EmployeeId { get; set; }

        public int? PlaceOfWorkId { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int CreatedByEmployeeId { get; set; }

        public int? ModifiedByEmployeeId { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public REmployeeDTO CreatedByEmployee { get; set; } = null!;
        public REmployeeDTO Employee { get; set; } = null!;
        public EmployeeShiftTypeDto EmployeeShiftType { get; set; } = null!;
        public REmployeeDTO? ModifiedByEmployee { get; set; }
        public RentalPlaceDto? PlaceOfWork { get; set; }
    }
}

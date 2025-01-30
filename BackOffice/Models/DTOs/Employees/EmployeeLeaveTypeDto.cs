using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeLeaveTypeDto : BaseDtoModel
    {
        public int EmployeeLeaveTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int DefaultDays { get; set; }
    }
}

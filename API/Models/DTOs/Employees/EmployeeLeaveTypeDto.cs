namespace API.Models.DTOs.Employees
{
    public class EmployeeLeaveTypeDto
    {
        public int EmployeeLeaveTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int DefaultDays { get; set; }
    }
}

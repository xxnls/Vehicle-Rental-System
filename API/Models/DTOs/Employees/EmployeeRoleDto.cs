namespace API.Models.DTOs.Employees
{
    public class EmployeeRoleDto
    {
        public int Id { get; set; } // Role ID (inherited from IdentityRole)
        public string Name { get; set; } = null!;// Role name (inherited from IdentityRole)
        public int RolePower { get; set; }
        public bool ManageVehicles { get; set; }
        public bool ManageEmployees { get; set; }
        public bool ManageRentals { get; set; }
        public bool ManageLeaves { get; set; }
        public bool ManageSchedule { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

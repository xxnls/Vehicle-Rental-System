using API.Interfaces;
using API.Models.DTOs.Other;
using API.Models.Employees;

namespace API.Models.DTOs.Employees
{
    public class EmployeeDto : IBaseModel
    {
        public int Id { get; set; }
        public int EmployeeStatisticsId { get; set; }
        public int EmployeeFinancesId { get; set; }
        public int RentalPlaceId { get; set; }
        public int AddressId { get; set; }
        public int EmployeePositionId { get; set; }
        public int? SupervisorId { get; set; }
        public string? Status { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; } // Added for creation
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties
        public AddressDto? Address { get; set; } = null!; 
        public EmployeeFinanceDto? EmployeeFinances { get; set; } = null!;
        public EmployeePositionDto? EmployeePosition { get; set; } = null!;
        public EmployeeStatisticsDto? EmployeeStatistics { get; set; } = null!;
        public RentalPlaceDto? RentalPlace { get; set; } = null!;
        public EmployeeSelectorDto? Supervisor { get; set; } = null!;
    }
}

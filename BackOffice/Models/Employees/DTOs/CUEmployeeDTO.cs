namespace BackOffice.Models.Employees.DTOs
{
    public class CUEmployeeDTO
    {
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

        public string Password { get; set; }
    }
}

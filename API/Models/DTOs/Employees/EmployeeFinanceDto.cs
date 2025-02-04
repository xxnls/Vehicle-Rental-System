namespace API.Models.DTOs.Employees
{
    public class EmployeeFinanceDto
    {
        public int EmployeeFinancesId { get; set; }

        public decimal? BaseSalary { get; set; }

        public decimal? HourlyRate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

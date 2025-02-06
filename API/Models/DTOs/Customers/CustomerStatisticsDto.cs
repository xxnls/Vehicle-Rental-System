namespace API.Models.DTOs.Customers
{
    public class CustomerStatisticsDto
    {
        public int CustomerStatisticsId { get; set; }

        public int TotalRentals { get; set; }

        public short ActiveRentals { get; set; }

        public int CanceledRentals { get; set; }

        public DateTime? FirstRentalDate { get; set; }

        public DateTime? LastRentalDate { get; set; }
    }
}

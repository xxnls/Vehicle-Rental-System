using API.Models.DTOs.Customers;
using API.Models.DTOs.Vehicles;

namespace API.Interfaces
{
    public interface IRentalCostCalculation
    {
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        CustomerDto Customer { get; }
        VehicleDto Vehicle { get; }
    }
}

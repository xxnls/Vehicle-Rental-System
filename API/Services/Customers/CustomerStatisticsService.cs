using API.Context;
using API.Interfaces;
using API.Models.Customers;
using API.Models.DTOs.Customers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services.Customers
{
    public class CustomerStatisticsService : BaseApiService<CustomerStatistic, CustomerStatisticsDto, CustomerStatisticsDto>
    {
        private readonly ApiDbContext _context;

        public CustomerStatisticsService(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        protected override Expression<Func<CustomerStatistic, bool>> BuildSearchQuery(string search)
        {
            return cs =>
                cs.CustomerStatisticsId.ToString().Contains(search) ||
                cs.TotalRentals.ToString().Contains(search) ||
                cs.ActiveRentals.ToString().Contains(search) ||
                cs.CanceledRentals.ToString().Contains(search);
        }

        protected override void UpdateEntity(CustomerStatistic entity, CustomerStatisticsDto model)
        {
            entity.TotalRentals = model.TotalRentals;
            entity.ActiveRentals = model.ActiveRentals;
            entity.CanceledRentals = model.CanceledRentals;
            entity.FirstRentalDate = model.FirstRentalDate;
            entity.LastRentalDate = model.LastRentalDate;
        }

        #region Mapping

        public override CustomerStatistic MapToEntity(CustomerStatisticsDto model)
        {
            return new CustomerStatistic
            {
                CustomerStatisticsId = model.CustomerStatisticsId,
                TotalRentals = model.TotalRentals,
                ActiveRentals = model.ActiveRentals,
                CanceledRentals = model.CanceledRentals,
                FirstRentalDate = model.FirstRentalDate,
                LastRentalDate = model.LastRentalDate
            };
        }

        public override Expression<Func<CustomerStatistic, CustomerStatisticsDto>> MapToDto()
        {
            return cs => new CustomerStatisticsDto
            {
                CustomerStatisticsId = cs.CustomerStatisticsId,
                TotalRentals = cs.TotalRentals,
                ActiveRentals = cs.ActiveRentals,
                CanceledRentals = cs.CanceledRentals,
                FirstRentalDate = cs.FirstRentalDate,
                LastRentalDate = cs.LastRentalDate
            };
        }

        public override CustomerStatisticsDto MapSingleEntityToDto(CustomerStatistic entity)
        {
            return new CustomerStatisticsDto
            {
                CustomerStatisticsId = entity.CustomerStatisticsId,
                TotalRentals = entity.TotalRentals,
                ActiveRentals = entity.ActiveRentals,
                CanceledRentals = entity.CanceledRentals,
                FirstRentalDate = entity.FirstRentalDate,
                LastRentalDate = entity.LastRentalDate
            };
        }

        #endregion

        public override async Task<CustomerStatistic> FindEntityById(int id)
        {
            return await _context.CustomerStatistics
                .FirstOrDefaultAsync(cs => cs.CustomerStatisticsId == id);
        }
    }
}
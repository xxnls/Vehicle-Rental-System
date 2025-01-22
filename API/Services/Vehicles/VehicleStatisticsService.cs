using API.Context;
using API.Models.DTOs.Vehicles;
using System.Linq.Expressions;
using API.Models.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Vehicles
{
    public class
        VehicleStatisticsService : BaseApiService<VehicleStatistic, VehicleStatisticsDto, VehicleStatisticsDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public VehicleStatisticsService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<VehicleStatistic, bool>> BuildSearchQuery(string search)
        {
            return vs =>
                vs.VehicleStatisticsId.ToString().Contains(search) ||
                vs.TotalRentals.ToString().Contains(search) ||
                vs.RentalRevenue.ToString().Contains(search) ||
                (vs.FirstRentalDate.HasValue && vs.FirstRentalDate.Value.ToString().Contains(search)) ||
                (vs.LastRentalDate.HasValue && vs.LastRentalDate.Value.ToString().Contains(search));
        }

        protected override Expression<Func<VehicleStatistic, bool>> GetActiveFilter(bool showDeleted)
        {
            return vs => true;
        }

        public override VehicleStatistic MapToEntity(VehicleStatisticsDto model)
        {
            return new VehicleStatistic
            {
                VehicleStatisticsId = model.VehicleStatisticsId,
                TotalRentals = model.TotalRentals,
                RentalRevenue = model.RentalRevenue,
                FirstRentalDate = model.FirstRentalDate,
                LastRentalDate = model.LastRentalDate
            };
        }

        public override Expression<Func<VehicleStatistic, VehicleStatisticsDto>> MapToDto()
        {
            return vs => new VehicleStatisticsDto
            {
                VehicleStatisticsId = vs.VehicleStatisticsId,
                TotalRentals = vs.TotalRentals,
                RentalRevenue = vs.RentalRevenue,
                FirstRentalDate = vs.FirstRentalDate,
                LastRentalDate = vs.LastRentalDate
            };
        }

        public override VehicleStatisticsDto MapSingleEntityToDto(VehicleStatistic entity)
        {
            return new VehicleStatisticsDto
            {
                VehicleStatisticsId = entity.VehicleStatisticsId,
                TotalRentals = entity.TotalRentals,
                RentalRevenue = entity.RentalRevenue,
                FirstRentalDate = entity.FirstRentalDate,
                LastRentalDate = entity.LastRentalDate
            };
        }

        public override async Task<VehicleStatisticsDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleStatistic).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleStatistic entity, VehicleStatisticsDto model)
        {
            entity.TotalRentals = model.TotalRentals;
            entity.RentalRevenue = model.RentalRevenue;
            entity.FirstRentalDate = model.FirstRentalDate;
            entity.LastRentalDate = model.LastRentalDate;
        }

        public override async Task<VehicleStatistic> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleStatistics
                .FirstOrDefaultAsync(vs => vs.VehicleStatisticsId == id);
        }

    }
}

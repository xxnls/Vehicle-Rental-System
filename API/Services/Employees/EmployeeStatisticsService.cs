using API.Context;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeStatisticsService : BaseApiService<EmployeeStatistic, EmployeeStatisticsDto, EmployeeStatisticsDto>
    {
        private readonly ApiDbContext _context;
        public EmployeeStatisticsService(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        protected override Expression<Func<EmployeeStatistic, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.EmployeeStatisticsId.ToString().Contains(search) ||
                e.TotalWorkDays.ToString().Contains(search) ||
                e.LateArrivals.ToString().Contains(search) ||
                e.EarlyDepartures.ToString().Contains(search) ||
                e.OvertimeHours.ToString().Contains(search) ||
                e.SickLeavesTaken.ToString().Contains(search) ||
                e.VacationDaysTaken.ToString().Contains(search) ||
                e.UnpaidLeavesTaken.ToString().Contains(search) ||
                (e.TotalRentalsApproved.HasValue && e.TotalRentalsApproved.Value.ToString().Contains(search));
        }

        public override EmployeeStatistic MapToEntity(EmployeeStatisticsDto dto)
        {
            return new EmployeeStatistic
            {
                EmployeeStatisticsId = dto.EmployeeStatisticsId,
                TotalWorkDays = dto.TotalWorkDays,
                LateArrivals = dto.LateArrivals,
                EarlyDepartures = dto.EarlyDepartures,
                OvertimeHours = dto.OvertimeHours,
                SickLeavesTaken = dto.SickLeavesTaken,
                VacationDaysTaken = dto.VacationDaysTaken,
                UnpaidLeavesTaken = dto.UnpaidLeavesTaken,
                TotalRentalsApproved = dto.TotalRentalsApproved
            };
        }

        public override Expression<Func<EmployeeStatistic, EmployeeStatisticsDto>> MapToDto()
        {
            return e => new EmployeeStatisticsDto
            {
                EmployeeStatisticsId = e.EmployeeStatisticsId,
                TotalWorkDays = e.TotalWorkDays,
                LateArrivals = e.LateArrivals,
                EarlyDepartures = e.EarlyDepartures,
                OvertimeHours = e.OvertimeHours,
                SickLeavesTaken = e.SickLeavesTaken,
                VacationDaysTaken = e.VacationDaysTaken,
                UnpaidLeavesTaken = e.UnpaidLeavesTaken,
                TotalRentalsApproved = e.TotalRentalsApproved
            };
        }

        public override EmployeeStatisticsDto MapSingleEntityToDto(EmployeeStatistic entity)
        {
            return new EmployeeStatisticsDto
            {
                EmployeeStatisticsId = entity.EmployeeStatisticsId,
                TotalWorkDays = entity.TotalWorkDays,
                LateArrivals = entity.LateArrivals,
                EarlyDepartures = entity.EarlyDepartures,
                OvertimeHours = entity.OvertimeHours,
                SickLeavesTaken = entity.SickLeavesTaken,
                VacationDaysTaken = entity.VacationDaysTaken,
                UnpaidLeavesTaken = entity.UnpaidLeavesTaken,
                TotalRentalsApproved = entity.TotalRentalsApproved
            };
        }

        protected override void UpdateEntity(EmployeeStatistic entity, EmployeeStatisticsDto dto)
        {
            entity.TotalWorkDays = dto.TotalWorkDays;
            entity.LateArrivals = dto.LateArrivals;
            entity.EarlyDepartures = dto.EarlyDepartures;
            entity.OvertimeHours = dto.OvertimeHours;
            entity.SickLeavesTaken = dto.SickLeavesTaken;
            entity.VacationDaysTaken = dto.VacationDaysTaken;
            entity.UnpaidLeavesTaken = dto.UnpaidLeavesTaken;
            entity.TotalRentalsApproved = dto.TotalRentalsApproved;
        }

        public override async Task<EmployeeStatistic> FindEntityById(int id)
        {
            return await _context.EmployeeStatistics
                .FirstOrDefaultAsync(e => e.EmployeeStatisticsId == id);
        }
    }
}

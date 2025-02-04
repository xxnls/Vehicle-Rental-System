using API.Context;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Employees
{
    public class EmployeeFinancesService : BaseApiService<EmployeeFinance, EmployeeFinanceDto, EmployeeFinanceDto>
    {
        private readonly ApiDbContext _context;

        public EmployeeFinancesService(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        protected override Expression<Func<EmployeeFinance, bool>> BuildSearchQuery(string search)
        {
            return e =>
                e.EmployeeFinancesId.ToString().Contains(search) ||
                (e.BaseSalary.HasValue && e.BaseSalary.Value.ToString().Contains(search)) ||
                (e.HourlyRate.HasValue && e.HourlyRate.Value.ToString().Contains(search));
        }

        public override EmployeeFinance MapToEntity(EmployeeFinanceDto dto)
        {
            return new EmployeeFinance
            {
                EmployeeFinancesId = dto.EmployeeFinancesId,
                BaseSalary = dto.BaseSalary,
                HourlyRate = dto.HourlyRate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public override Expression<Func<EmployeeFinance, EmployeeFinanceDto>> MapToDto()
        {
            return e => new EmployeeFinanceDto
            {
                EmployeeFinancesId = e.EmployeeFinancesId,
                BaseSalary = e.BaseSalary,
                HourlyRate = e.HourlyRate,
                ModifiedDate = e.ModifiedDate
            };
        }

        public override EmployeeFinanceDto MapSingleEntityToDto(EmployeeFinance entity)
        {
            return new EmployeeFinanceDto
            {
                EmployeeFinancesId = entity.EmployeeFinancesId,
                BaseSalary = entity.BaseSalary,
                HourlyRate = entity.HourlyRate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        protected override void UpdateEntity(EmployeeFinance entity, EmployeeFinanceDto dto)
        {
            entity.BaseSalary = dto.BaseSalary;
            entity.HourlyRate = dto.HourlyRate;
            entity.ModifiedDate = DateTime.UtcNow;
        }

        public override async Task<EmployeeFinance> FindEntityById(int id)
        {
            return await _context.EmployeeFinances
                .FirstOrDefaultAsync(e => e.EmployeeFinancesId == id);
        }
    }
}

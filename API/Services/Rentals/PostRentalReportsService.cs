using API.Context;
using API.Models.Rentals;
using API.Models.DTOs.Rentals;
using API.Models.DTOs.Employees; // Important: Include the DTO namespace
using API.Services.Employees; // Important: Include the EmployeesService namespace
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace API.Services.PostRentalReports
{
    public class PostRentalReportsService : BaseApiService<PostRentalReport, PostRentalReportDto, PostRentalReportDto>
    {
        private readonly ApiDbContext _context;
        private readonly EmployeesService _employeesService; // Inject EmployeesService

        public PostRentalReportsService(ApiDbContext context, EmployeesService employeesService) : base(context)
        {
            _context = context;
            _employeesService = employeesService;
        }

        protected override Expression<Func<PostRentalReport, bool>> BuildSearchQuery(string search)
        {
            return r =>
                r.PostRentalReportId.ToString().Contains(search) ||
                r.InspectorEmployee.FirstName.Contains(search) ||
                r.InspectorEmployee.LastName.Contains(search);
        }

        #region Mapping

        public override PostRentalReport MapToEntity(PostRentalReportDto model)
        {
            return new PostRentalReport
            {
                PostRentalReportId = model.PostRentalReportId,
                InspectorEmployeeId = model.InspectorEmployeeId,
                IsCustomerLate = model.IsCustomerLate,
                IsCarDamaged = model.IsCarDamaged,
                IsCarRefueled = model.IsCarRefueled,
                CreatedDate = model.CreatedDate,
            };
        }

        public override Expression<Func<PostRentalReport, PostRentalReportDto>> MapToDto()
        {
            return r => new PostRentalReportDto
            {
                PostRentalReportId = r.PostRentalReportId,
                InspectorEmployeeId = r.InspectorEmployeeId,
                InspectorEmployee = r.InspectorEmployee != null ? _employeesService.MapSingleEntityToDto(r.InspectorEmployee) : null, // Map Employee
                IsCustomerLate = r.IsCustomerLate,
                IsCarDamaged = r.IsCarDamaged,
                IsCarRefueled = r.IsCarRefueled,
                CreatedDate = r.CreatedDate,
            };
        }

        public override PostRentalReportDto MapSingleEntityToDto(PostRentalReport entity)
        {
            return new PostRentalReportDto
            {
                PostRentalReportId = entity.PostRentalReportId,
                InspectorEmployeeId = entity.InspectorEmployeeId,
                InspectorEmployee = entity.InspectorEmployee != null ? _employeesService.MapSingleEntityToDto(entity.InspectorEmployee) : null, // Map Employee
                IsCustomerLate = entity.IsCustomerLate,
                IsCarDamaged = entity.IsCarDamaged,
                IsCarRefueled = entity.IsCarRefueled,
                CreatedDate = entity.CreatedDate,
            };
        }

        #endregion

        public override async Task<PostRentalReport> FindEntityById(int id)
        {
            return await _context.PostRentalReports
                .Include(r => r.InspectorEmployee)
                .FirstOrDefaultAsync(r => r.PostRentalReportId == id);
        }

        protected override IQueryable<PostRentalReport> IncludeRelatedEntities(IQueryable<PostRentalReport> query)
        {
            return query.Include(r => r.InspectorEmployee);
        }

        public override async Task<PostRentalReportDto> CreateAsync(PostRentalReportDto postRentalReportDto)
        {
            try
            {
                var postRentalReport = new PostRentalReport
                {
                    InspectorEmployeeId = postRentalReportDto.InspectorEmployee.Id,
                    IsCustomerLate = postRentalReportDto.IsCustomerLate,
                    IsCarDamaged = postRentalReportDto.IsCarDamaged,
                    IsCarRefueled = postRentalReportDto.IsCarRefueled,
                    CreatedDate = DateTime.Now,
                };

                _context.PostRentalReports.Add(postRentalReport);
                await _context.SaveChangesAsync();

                return MapSingleEntityToDto(postRentalReport);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the Post Rental Report.", ex);
            }
        }

        protected override void UpdateEntity(PostRentalReport entity, PostRentalReportDto model)
        {
            entity.IsCustomerLate = model.IsCustomerLate;
            entity.IsCarDamaged = model.IsCarDamaged;
            entity.IsCarRefueled = model.IsCarRefueled;

            if (model.InspectorEmployee != null)
            {
                entity.InspectorEmployeeId = model.InspectorEmployee.Id;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var postRentalReport = await FindEntityById(id);
                if (postRentalReport == null)
                    return false;

                var postRentalReportDeleted = await base.DeleteAsync(id);
                if (!postRentalReportDeleted)
                    return false;

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
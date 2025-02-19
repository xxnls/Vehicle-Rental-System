using API.Context;
using API.Models;
using API.Models.DTOs;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Other;
using API.Models.Other;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.BusinessLogic;
using API.Services.Customers;
using API.Services.Employees;
using API.Services.FileSystem;

namespace API.Services.Other
{
    public class LicenseApprovalRequestsService : BaseApiService<LicenseApprovalRequests, LicenseApprovalRequestsDto, LicenseApprovalRequestsDto>
    {
        private readonly ApiDbContext _context;
        private readonly CustomersService _customersService;
        private readonly EmployeesService _employeesService;
        private readonly FileSystemService _documentsService;

        public LicenseApprovalRequestsService(
            ApiDbContext context,
            CustomersService customersService,
            EmployeesService employeesService,
            FileSystemService documentsService) : base(context)
        {
            _context = context;
            _customersService = customersService;
            _employeesService = employeesService;
            _documentsService = documentsService;
        }

        protected override Expression<Func<LicenseApprovalRequests, bool>> BuildSearchQuery(string search)
        {
            return r =>
                r.LicenseApprovalRequestId.ToString().Contains(search) ||
                r.Customer.FirstName.Contains(search) ||
                r.Customer.LastName.Contains(search) ||
                r.LicenseType.Contains(search) ||
                r.RequestStatus.Contains(search) ||
                r.CreatedDate.ToString().Contains(search) ||
                r.ModifiedDate.ToString().Contains(search) ||
                r.DeletedDate.ToString().Contains(search);
        }

        protected override Expression<Func<LicenseApprovalRequests, bool>> GetActiveFilter(bool showDeleted)
        {
            return r => r.IsActive != showDeleted;
        }

        #region Mapping

        public override LicenseApprovalRequests MapToEntity(LicenseApprovalRequestsDto model)
        {
            return new LicenseApprovalRequests
            {
                LicenseApprovalRequestId = model.LicenseApprovalRequestId,
                CustomerId = model.CustomerId,
                ApprovedByEmployeeId = model.ApprovedByEmployeeId,
                DocumentId = model.DocumentId,
                LicenseType = model.LicenseType,
                RequestStatus = model.RequestStatus,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<LicenseApprovalRequests, LicenseApprovalRequestsDto>> MapToDto()
        {
            return r => new LicenseApprovalRequestsDto
            {
                LicenseApprovalRequestId = r.LicenseApprovalRequestId,
                CustomerId = r.CustomerId,
                Customer = r.Customer != null ? _customersService.MapSingleEntityToDto(r.Customer) : null,
                ApprovedByEmployeeId = r.ApprovedByEmployeeId,
                ApprovedByEmployee = r.ApprovedByEmployee != null ? _employeesService.MapSingleEntityToDto(r.ApprovedByEmployee) : null,
                DocumentId = r.DocumentId,
                Document = r.Document != null ? _documentsService.MapSingleEntityToDto(r.Document) : null,
                LicenseType = r.LicenseType,
                RequestStatus = r.RequestStatus,
                CreatedDate = r.CreatedDate,
                ModifiedDate = r.ModifiedDate,
                DeletedDate = r.DeletedDate,
                IsActive = r.IsActive
            };
        }

        public override LicenseApprovalRequestsDto MapSingleEntityToDto(LicenseApprovalRequests entity)
        {
            return new LicenseApprovalRequestsDto
            {
                LicenseApprovalRequestId = entity.LicenseApprovalRequestId,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer != null ? _customersService.MapSingleEntityToDto(entity.Customer) : null,
                ApprovedByEmployeeId = entity.ApprovedByEmployeeId,
                ApprovedByEmployee = entity.ApprovedByEmployee != null ? _employeesService.MapSingleEntityToDto(entity.ApprovedByEmployee) : null,
                DocumentId = entity.DocumentId,
                Document = entity.Document != null ? _documentsService.MapSingleEntityToDto(entity.Document) : null,
                LicenseType = entity.LicenseType,
                RequestStatus = entity.RequestStatus,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        #endregion

        public override async Task<LicenseApprovalRequestsDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(LicenseApprovalRequests).Name} not found");

            return MapSingleEntityToDto(entity);
        }

        //public async Task<PaginatedResult<LicenseApprovalRequestsDto>> GetCustomerLicenseApprovalRequestsAsync(int id)
        //{
        //    var query = _context.LicenseApprovalRequests
        //        .Where(r => r.CustomerId == id && r.IsActive);
        //    return await GetAllAsync(preFilteredQuery: query);
        //}

        public async Task<PaginatedResult<LicenseApprovalRequestsDto>> GetPendingLicenseApprovalRequestsAsync(
            string? search = null,
            int page = 1,
            int pageSize = 10,
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? modifiedBefore = null,
            DateTime? modifiedAfter = null)
        {
            var query = _context.LicenseApprovalRequests
                .Where(r => r.RequestStatus == RequestStatus.Pending.ToString() && r.IsActive);

            return await GetAllAsync(
                search, page, false, createdBefore, createdAfter, modifiedBefore, modifiedAfter, pageSize, query);
        }

        protected override void UpdateEntity(LicenseApprovalRequests entity, LicenseApprovalRequestsDto model)
        {
            entity.CustomerId = model.CustomerId;
            entity.ApprovedByEmployeeId = model.ApprovedByEmployeeId;
            entity.DocumentId = model.DocumentId;
            entity.LicenseType = model.LicenseType;
            entity.RequestStatus = model.RequestStatus;

            // Update navigation properties if provided
            //if (model.Customer != null)
            //{
            //    entity.CustomerId = model.Customer.Id;
            //}

            //if (model.ApprovedByEmployee != null)
            //{
            //    entity.ApprovedByEmployeeId = model.ApprovedByEmployee.Id;
            //}

            //if (model.Document != null)
            //{
            //    entity.DocumentId = model.Document.DocumentId;
            //}

            // Restore the entity
            if (model.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        public override async Task<LicenseApprovalRequests> FindEntityById(int id)
        {
            return await _context.LicenseApprovalRequests
                .Include(r => r.Customer)
                .Include(r => r.Customer.CustomerType)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.ApprovedByEmployee)
                .Include(r => r.Document)
                .Include(r => r.Document.Customer)
                .Include(r => r.Document.Customer.Address)
                .Include(r => r.Document.Customer.Address.Country)
                .FirstOrDefaultAsync(r => r.LicenseApprovalRequestId == id);
        }

        protected override IQueryable<LicenseApprovalRequests> IncludeRelatedEntities(IQueryable<LicenseApprovalRequests> query)
        {
            return query
                .Include(r => r.Customer)
                .Include(r => r.Customer.CustomerType)
                .Include(r => r.Customer.Address)
                .Include(r => r.Customer.Address.Country)
                .Include(r => r.ApprovedByEmployee)
                .Include(r => r.Document)
                .Include(r => r.Document.Customer)
                .Include(r => r.Document.Customer.Address)
                .Include(r => r.Document.Customer.Address.Country);
        }

        public override async Task<LicenseApprovalRequestsDto> CreateAsync(LicenseApprovalRequestsDto licenseApprovalRequestsDto)
        {
            try
            {
                var licenseApprovalRequest = new LicenseApprovalRequests
                {
                    CustomerId = licenseApprovalRequestsDto.Customer.Id,
                    DocumentId = licenseApprovalRequestsDto.Document.DocumentId,
                    LicenseType = licenseApprovalRequestsDto.LicenseType,
                    RequestStatus = RequestStatus.Pending.ToString(), // Set initial status
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                };

                _context.LicenseApprovalRequests.Add(licenseApprovalRequest);
                await _context.SaveChangesAsync();

                return MapSingleEntityToDto(licenseApprovalRequest);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the license approval request.", ex);
            }
        }

        // Custom method for soft deleting a license approval request
        public override async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var licenseApprovalRequest = await FindEntityById(id);
                if (licenseApprovalRequest == null)
                    return false;

                var licenseApprovalRequestDeleted = await base.DeleteAsync(id);
                if (!licenseApprovalRequestDeleted)
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
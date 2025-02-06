using API.Context;
using API.Models;
using API.Models.DTOs.Customers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.Models.Customers;

namespace API.Services.Customers
{
    public class CustomerTypesService : BaseApiService<CustomerType, CustomerTypeDto, CustomerTypeDto>
    {
        private readonly ApiDbContext _context;

        public CustomerTypesService(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        // Build a search query for customer types
        protected override Expression<Func<CustomerType, bool>> BuildSearchQuery(string search)
        {
            return ct =>
                ct.CustomerTypeId.ToString().Contains(search) || // CustomerType ID
                ct.CustomerType1.Contains(search) || // CustomerType name
                ct.MaxRentals.ToString().Contains(search) || // Max rentals
                ct.DiscountPercent.ToString().Contains(search); // Discount percent
        }

        // Get the active filter for customer types
        protected override Expression<Func<CustomerType, bool>> GetActiveFilter(bool showDeleted)
        {
            return ct => ct.IsActive != showDeleted;
        }

        #region Mapping

        // Map a DTO to a CustomerType entity
        public override CustomerType MapToEntity(CustomerTypeDto dto)
        {
            return new CustomerType
            {
                CustomerTypeId = dto.CustomerTypeId,
                CustomerType1 = dto.CustomerType,
                MaxRentals = dto.MaxRentals,
                DiscountPercent = dto.DiscountPercent,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate,
                DeletedDate = dto.DeletedDate,
                IsActive = dto.IsActive
            };
        }

        // Map a CustomerType entity to a DTO
        public override Expression<Func<CustomerType, CustomerTypeDto>> MapToDto()
        {
            return ct => new CustomerTypeDto
            {
                CustomerTypeId = ct.CustomerTypeId,
                CustomerType = ct.CustomerType1,
                MaxRentals = ct.MaxRentals,
                DiscountPercent = ct.DiscountPercent,
                CreatedDate = ct.CreatedDate,
                ModifiedDate = ct.ModifiedDate,
                DeletedDate = ct.DeletedDate,
                IsActive = ct.IsActive
            };
        }

        // Map a single CustomerType entity to a DTO
        public override CustomerTypeDto MapSingleEntityToDto(CustomerType entity)
        {
            return new CustomerTypeDto
            {
                CustomerTypeId = entity.CustomerTypeId,
                CustomerType = entity.CustomerType1,
                MaxRentals = entity.MaxRentals,
                DiscountPercent = entity.DiscountPercent,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        #endregion

        // Find a CustomerType entity by its ID
        public override async Task<CustomerType> FindEntityById(int id)
        {
            return await _context.CustomerTypes
                .FirstOrDefaultAsync(ct => ct.CustomerTypeId == id);
        }

        // Update a CustomerType entity with data from a DTO
        protected override void UpdateEntity(CustomerType entity, CustomerTypeDto dto)
        {
            entity.CustomerType1 = dto.CustomerType;
            entity.MaxRentals = dto.MaxRentals;
            entity.DiscountPercent = dto.DiscountPercent;

            if (dto.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }
    }
}
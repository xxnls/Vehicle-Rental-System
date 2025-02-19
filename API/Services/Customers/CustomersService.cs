using API.Context;
using API.Interfaces;
using API.Models.Customers;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Other;
using API.Services.Other;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;

namespace API.Services.Customers
{
    public class CustomersService : BaseApiService<Customer, CustomerDto, CustomerDto>
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<Customer> _customerManager;
        private readonly AddressesService _addressesService;
        private readonly CustomerStatisticsService _customerStatisticsService;
        private readonly CustomerTypesService _customerTypesService;

        public CustomersService(
            ApiDbContext context,
            AddressesService addressesService,
            CustomerStatisticsService customerStatisticsService,
            CustomerTypesService customerTypesService,
            UserManager<Customer> customerManager) : base(context)
        {
            _context = context;
            _addressesService = addressesService;
            _customerStatisticsService = customerStatisticsService;
            _customerTypesService = customerTypesService;
            _customerManager = customerManager;
        }

        protected override Expression<Func<Customer, bool>> BuildSearchQuery(string search)
        {
            return c =>
                c.Id.ToString().Contains(search) ||
                c.FirstName.Contains(search) ||
                c.LastName.Contains(search) ||
                c.Email.Contains(search) ||
                c.PhoneNumber.Contains(search) ||
                c.CompanyName.Contains(search) ||
                c.Address.City.Contains(search) ||
                c.Address.FirstLine.Contains(search) ||
                c.Address.SecondLine.Contains(search) ||
                c.Address.ZipCode.Contains(search);
        }

        protected override Expression<Func<Customer, bool>> GetActiveFilter(bool showDeleted)
        {
            return c => c.IsActive != showDeleted;
        }

        #region Mapping

        public override Customer MapToEntity(CustomerDto model)
        {
            return new Customer
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CompanyName = model.CompanyName,
                ApprovedA = model.ApprovedA ?? false,
                ApprovedB = model.ApprovedB ?? false,
                ApprovedC = model.ApprovedC ?? false,
                UserName = model.UserName,
                AddressId = model.AddressId,
                CustomerTypeId = model.CustomerTypeId,
                CustomerStatisticsId = model.CustomerStatisticsId,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<Customer, CustomerDto>> MapToDto()
        {
            return c => new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                CompanyName = c.CompanyName,
                UserName = c.UserName,
                AddressId = c.AddressId,
                ApprovedA = c.ApprovedA,
                ApprovedB = c.ApprovedB,
                ApprovedC = c.ApprovedC,
                CustomerTypeId = c.CustomerTypeId,
                CustomerStatisticsId = c.CustomerStatisticsId,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                DeletedDate = c.DeletedDate,
                IsActive = c.IsActive,
                Address = c.Address != null ? _addressesService.MapSingleEntityToDto(c.Address) : null,
                CustomerStatistics = c.CustomerStatistics != null ? _customerStatisticsService.MapSingleEntityToDto(c.CustomerStatistics) : null,
                CustomerType = c.CustomerType != null ? _customerTypesService.MapSingleEntityToDto(c.CustomerType) : null
            };
        }

        public override CustomerDto MapSingleEntityToDto(Customer entity)
        {
            return new CustomerDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                CompanyName = entity.CompanyName,
                UserName = entity.UserName,
                ApprovedA = entity.ApprovedA,
                ApprovedB = entity.ApprovedB,
                ApprovedC = entity.ApprovedC,
                AddressId = entity.AddressId,
                CustomerTypeId = entity.CustomerTypeId,
                CustomerStatisticsId = entity.CustomerStatisticsId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,
                Address = entity.Address != null ? _addressesService.MapSingleEntityToDto(entity.Address) : null,
                CustomerStatistics = entity.CustomerStatistics != null ? _customerStatisticsService.MapSingleEntityToDto(entity.CustomerStatistics) : null,
                CustomerType = entity.CustomerType != null ? _customerTypesService.MapSingleEntityToDto(entity.CustomerType) : null
            };
        }

        #endregion

        public override async Task<CustomerDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null) throw new KeyNotFoundException($"{typeof(Customer).Name} not found");

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Customer entity, CustomerDto model)
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.ApprovedA = model.ApprovedA ?? false;
            entity.ApprovedB = model.ApprovedB ?? false;
            entity.ApprovedC = model.ApprovedC ?? false;
            entity.PhoneNumber = model.PhoneNumber;
            entity.CompanyName = model.CompanyName;
            entity.ModifiedDate = DateTime.UtcNow;

            if (model.Address != null)
            {
                entity.Address.FirstLine = model.Address.FirstLine;
                entity.Address.SecondLine = model.Address.SecondLine;
                entity.Address.ZipCode = model.Address.ZipCode;
                entity.Address.City = model.Address.City;

                entity.Address.IsActive = model.IsActive;
                entity.Address.DeletedDate = model.IsActive ? null : model.DeletedDate;
                entity.Address.ModifiedDate = DateTime.UtcNow;

                if (model.Address.Country != null)
                {
                    entity.Address.CountryId = model.Address.Country.CountryId;
                }
            }

            if (model.CustomerType != null)
            {
                entity.CustomerTypeId = model.CustomerType.CustomerTypeId;
            }

            if (model.IsActive)
            {
                entity.DeletedDate = null;
                entity.IsActive = true;
            }
        }

        public override async Task<Customer> FindEntityById(int id)
        {
            return await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Address.Country)
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerStatistics)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        protected override IQueryable<Customer> IncludeRelatedEntities(IQueryable<Customer> query)
        {
            return query
                .Include(c => c.Address)
                .Include(c => c.Address.Country)
                .Include(c => c.CustomerType)
                .Include(c => c.CustomerStatistics);
        }

        public override async Task<CustomerDto> CreateAsync(CustomerDto customerDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (customerDto.Address == null)
                {
                    throw new ArgumentException("Address is required.");
                }

                var newAddress = new Address
                {
                    FirstLine = customerDto.Address.FirstLine,
                    City = customerDto.Address.City,
                    SecondLine = customerDto.Address.SecondLine,
                    ZipCode = customerDto.Address.ZipCode,
                    CountryId = customerDto.Address.Country.CountryId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();

                var customerStatisticsDto = new CustomerStatisticsDto
                {
                    TotalRentals = 0,
                    ActiveRentals = 0,
                    CanceledRentals = 0
                };

                var createdStatistics = await _customerStatisticsService.CreateAsync(customerStatisticsDto);
                await _context.SaveChangesAsync();

                var generatedUserName = await GenerateUniqueUsernameAsync(customerDto.FirstName, customerDto.LastName);

                var customer = new Customer
                {
                    UserName = generatedUserName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    CompanyName = customerDto.CompanyName,
                    AddressId = newAddress.AddressId,
                    CustomerTypeId = customerDto.CustomerType?.CustomerTypeId ?? throw new ArgumentException("CustomerType is required."),
                    CustomerStatisticsId = createdStatistics.CustomerStatisticsId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                // Create the customer in the Identity system
                var result = await _customerManager.CreateAsync(customer, customerDto.Password);

                if (!result.Succeeded)
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Customer creation failed: {errorMessages}");
                }

                // Assign the "Customer" role to the user
                //if (await _roleManager.RoleExistsAsync("Customer"))
                //{
                //    await _customerManager.AddToRoleAsync(customer, "Customer");
                //}

                await transaction.CommitAsync();

                return MapSingleEntityToDto(customer);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while creating the customer.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get the customer entity with its related IDs
                var customer = await FindEntityById(id);
                if (customer == null)
                    return false;

                // Delete customer using base implementation
                var customerDeleted = await base.DeleteAsync(id);
                if (!customerDeleted)
                    return false;

                // Delete associated address
                await _addressesService.DeleteAsync(customer.AddressId);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateUniqueUsernameAsync(string firstName, string lastName)
        {
            var rnd = new Random();
            var baseUsername = $"{RemoveDiacritics(firstName.ToLower())}.{RemoveDiacritics(lastName.ToLower())}";
            var generatedUsername = $"{baseUsername}{rnd.Next(1000):000}";

            // Ensure the username is unique
            while (await _context.Customers.AnyAsync(e => e.UserName == generatedUsername))
            {
                generatedUsername = $"{baseUsername}{rnd.Next(1000):000}";
            }

            return generatedUsername;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
using API.Context;
using API.Models.DTOs.Other;
using System.Linq.Expressions;
using API.Models;
using API.Models.Other;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Other
{
    public class AddressesService : BaseApiService<Address, AddressDto, AddressDto>
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly CountriesService _countriesService;

        public AddressesService(ApiDbContext context, CountriesService countriesService) : base(context)
        {
            _apiDbContext = context;
            _countriesService = countriesService;
        }

        protected override Expression<Func<Address, bool>> BuildSearchQuery(string search)
        {
            return a =>
                a.FirstLine.Contains(search) ||
                (a.SecondLine != null && a.SecondLine.Contains(search)) ||
                a.ZipCode.Contains(search) ||
                a.City.Contains(search) ||
                a.Country.Name.Contains(search) ||
                a.Country.Abbreviation.Contains(search);
        }

        protected override Expression<Func<Address, bool>> GetActiveFilter(bool showDeleted)
        {
            return a => a.IsActive != showDeleted;
        }

        public override Address MapToEntity(AddressDto model)
        {
            return new Address
            {
                AddressId = model.AddressId,
                FirstLine = model.FirstLine,
                SecondLine = model.SecondLine,
                ZipCode = model.ZipCode,
                City = model.City,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive,
                Country = _countriesService.MapToEntity(model.Country)
            };
        }

        public override Expression<Func<Address, AddressDto>> MapToDto()
        {
            return a => new AddressDto
            {
                AddressId = a.AddressId,
                FirstLine = a.FirstLine,
                SecondLine = a.SecondLine,
                ZipCode = a.ZipCode,
                City = a.City,
                CreatedDate = a.CreatedDate,
                ModifiedDate = a.ModifiedDate,
                DeletedDate = a.DeletedDate,
                IsActive = a.IsActive,

                Country = _countriesService.MapSingleEntityToDto(a.Country)
            };
        }

        public override AddressDto MapSingleEntityToDto(Address entity)
        {
            return new AddressDto
            {
                AddressId = entity.AddressId,
                FirstLine = entity.FirstLine,
                SecondLine = entity.SecondLine,
                ZipCode = entity.ZipCode,
                City = entity.City,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,

                Country = _countriesService.MapSingleEntityToDto(entity.Country)
            };
        }

        public override async Task<AddressDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(Address).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Address entity, AddressDto model)
        {
            entity.FirstLine = model.FirstLine;
            entity.SecondLine = model.SecondLine;
            entity.ZipCode = model.ZipCode;
            entity.City = model.City;
            entity.Country = _countriesService.MapToEntity(model.Country);

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<Address> FindEntityById(int id)
        {
            return await _apiDbContext.Addresses
                .Include(a => a.Country)
                .FirstOrDefaultAsync(a => a.AddressId == id);
        }
    }
}

using API.Context;
using API.Models.DTOs.Other;
using System.Linq.Expressions;
using API.Models.Other;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Other
{
    public class CountriesService : BaseApiService<Country, CountryDto, CountryDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public CountriesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<Country, bool>> BuildSearchQuery(string search)
        {
            return c =>
                c.CountryId.ToString().Contains(search) ||
                c.Name.Contains(search) ||
                c.FullName.Contains(search) ||
                c.Abbreviation.Contains(search) ||
                c.DialingCode.Contains(search);
        }

        public override Country MapToEntity(CountryDto model)
        {
            return new Country
            {
                CountryId = model.CountryId,
                Name = model.Name,
                FullName = model.FullName,
                Abbreviation = model.Abbreviation,
                DialingCode = model.DialingCode
            };
        }

        public override Expression<Func<Country, CountryDto>> MapToDto()
        {
            return c => new CountryDto
            {
                CountryId = c.CountryId,
                Name = c.Name,
                FullName = c.FullName,
                Abbreviation = c.Abbreviation,
                DialingCode = c.DialingCode
            };
        }

        public override CountryDto MapSingleEntityToDto(Country entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CountryDto
            {
                CountryId = entity.CountryId,
                Name = entity.Name,
                FullName = entity.FullName,
                Abbreviation = entity.Abbreviation,
                DialingCode = entity.DialingCode
            };
        }

        public override async Task<CountryDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(Country).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Country entity, CountryDto model)
        {
            entity.Name = model.Name;
            entity.FullName = model.FullName;
            entity.Abbreviation = model.Abbreviation;
            entity.DialingCode = model.DialingCode;
        }

        public override async Task<Country> FindEntityById(int id)
        {
            return await _apiDbContext.Countries
                .FirstOrDefaultAsync(c => c.CountryId == id);
        }

        // Method for seeder
        public async Task AddCountryAsync(CountryDto countryDto)
        {
            var country = MapToEntity(countryDto);
            await _apiDbContext.Countries.AddAsync(country);
            await _apiDbContext.SaveChangesAsync();
        }
    }
}

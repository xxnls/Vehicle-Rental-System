using API.Context;
using API.Models.DTOs.Other;
using System.Linq.Expressions;
using API.Models.Other;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Other
{
    public class RentalPlacesService : BaseApiService<RentalPlace, RentalPlaceDto, RentalPlaceDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public RentalPlacesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<RentalPlace, bool>> BuildSearchQuery(string search)
        {
            return rp =>
                rp.RentalPlaceId.ToString().Contains(search) ||
                rp.LocationId.ToString().Contains(search) ||
                rp.AddressId.ToString().Contains(search) ||
                rp.Address.City.Contains(search) ||
                rp.Address.FirstLine.Contains(search) ||
                (rp.Address.SecondLine != null && rp.Address.SecondLine.Contains(search));
        }

        protected override Expression<Func<RentalPlace, bool>> GetActiveFilter(bool showDeleted)
        {
            return rp => rp.IsActive != showDeleted;
        }

        protected override RentalPlace MapToEntity(RentalPlaceDto model)
        {
            return new RentalPlace
            {
                RentalPlaceId = model.RentalPlaceId,
                LocationId = model.LocationId,
                AddressId = model.AddressId,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive
            };
        }

        // Map Entity to DTO
        protected override Expression<Func<RentalPlace, RentalPlaceDto>> MapToDto()
        {
            return rp => new RentalPlaceDto
            {
                RentalPlaceId = rp.RentalPlaceId,
                LocationId = rp.LocationId,
                Gpslatitude = rp.Location.Gpslatitude,
                Gpslongitude = rp.Location.Gpslongitude,
                AddressId = rp.AddressId,
                City = rp.Address.City,
                FirstLine = rp.Address.FirstLine,
                SecondLine = rp.Address.SecondLine,
                CreatedDate = rp.CreatedDate,
                ModifiedDate = rp.ModifiedDate,
                DeletedDate = rp.DeletedDate,
                IsActive = rp.IsActive
            };
        }

        // Map a single entity to DTO
        protected override RentalPlaceDto MapSingleEntityToDto(RentalPlace entity)
        {
            return new RentalPlaceDto
            {
                RentalPlaceId = entity.RentalPlaceId,
                LocationId = entity.LocationId,
                Gpslatitude = entity.Location.Gpslatitude,
                Gpslongitude = entity.Location.Gpslongitude,
                AddressId = entity.AddressId,
                City = entity.Address.City,
                FirstLine = entity.Address.FirstLine,
                SecondLine = entity.Address.SecondLine,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive
            };
        }

        // Find entity by ID
        protected override async Task<RentalPlace> FindEntityById(int id)
        {
            return await _apiDbContext.RentalPlaces
                .Include(rp => rp.Location)
                .Include(rp => rp.Address)
                .FirstOrDefaultAsync(rp => rp.RentalPlaceId == id);
        }

        // Update entity
        protected override void UpdateEntity(RentalPlace entity, RentalPlaceDto model)
        {
            entity.LocationId = model.LocationId;
            entity.AddressId = model.AddressId;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        // Override GetByIdAsync if additional logic is needed
        public override async Task<RentalPlaceDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{nameof(RentalPlace)} not found");
            }

            return MapSingleEntityToDto(entity);
        }
    }
}

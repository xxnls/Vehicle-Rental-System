using API.Context;
using API.Models.DTOs.Other;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using API.Models.Other;

namespace API.Services.Other
{
    public class LocationsService : BaseApiService<Location, LocationDto, LocationDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public LocationsService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<Location, bool>> BuildSearchQuery(string search)
        {
            return l =>
                l.LocationId.ToString().Contains(search) ||
                l.Gpslatitude.ToString().Contains(search) ||
                l.Gpslongitude.ToString().Contains(search) ||
                l.DateTime.ToString().Contains(search) ||
                l.IsActive.ToString().Contains(search);
        }

        protected override Expression<Func<Location, bool>> GetActiveFilter(bool showDeleted)
        {
            return l => l.IsActive != showDeleted;
        }

        public override Location MapToEntity(LocationDto model)
        {
            return new Location
            {
                LocationId = model.LocationId,
                VehicleId = model.VehicleId,
                RentalPlaceId = model.RentalPlaceId,
                Gpslatitude = model.Gpslatitude,
                Gpslongitude = model.Gpslongitude,
                DateTime = model.DateTime,
                IsActive = model.IsActive
            };
        }

        public override Expression<Func<Location, LocationDto>> MapToDto()
        {
            return l => new LocationDto
            {
                LocationId = l.LocationId,
                VehicleId = l.VehicleId,
                RentalPlaceId = l.RentalPlaceId,
                Gpslatitude = l.Gpslatitude,
                Gpslongitude = l.Gpslongitude,
                DateTime = l.DateTime,
                IsActive = l.IsActive
            };
        }

        public override LocationDto MapSingleEntityToDto(Location entity)
        {
            return new LocationDto
            {
                LocationId = entity.LocationId,
                VehicleId = entity.VehicleId,
                RentalPlaceId = entity.RentalPlaceId,
                Gpslatitude = entity.Gpslatitude,
                Gpslongitude = entity.Gpslongitude,
                DateTime = entity.DateTime,
                IsActive = entity.IsActive
            };
        }

        public override async Task<LocationDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(Location).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Location entity, LocationDto model)
        {
            entity.VehicleId = model.VehicleId;
            entity.RentalPlaceId = model.RentalPlaceId;
            entity.Gpslatitude = model.Gpslatitude;
            entity.Gpslongitude = model.Gpslongitude;
            entity.DateTime = model.DateTime;
            entity.IsActive = model.IsActive;
        }

        public override async Task<Location> FindEntityById(int id)
        {
            return await _apiDbContext.Locations
                .FirstOrDefaultAsync(l => l.LocationId == id);
        }

    }
}

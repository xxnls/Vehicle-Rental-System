using API.Context;
using API.Models.DTOs.Other;
using System.Linq.Expressions;
using API.Models;
using API.Models.Other;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Other
{
    public class RentalPlacesService : BaseApiService<RentalPlace, RentalPlaceDto, RentalPlaceDto>
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly LocationsService _locationsService;
        private readonly AddressesService _addressesService;

        public RentalPlacesService(ApiDbContext context, LocationsService locationsService, AddressesService addressesService) : base(context)
        {
            _apiDbContext = context;
            _locationsService = locationsService;
            _addressesService = addressesService;
        }

        public override async Task<RentalPlaceDto> CreateAsync(RentalPlaceDto createDto)
        {
            var location = new LocationDto
            {
                Gpslatitude = createDto.Gpslatitude,
                Gpslongitude = createDto.Gpslongitude
            };

            var address = new AddressDto
            {
                City = createDto.City,
                FirstLine = createDto.FirstLine,
                SecondLine = createDto.SecondLine,
                ZipCode = createDto.ZipCode,
                CountryId = createDto.CountryId
            };

            var createdLocation = await _locationsService.CreateAsync(location);
            var createdAddress = await _addressesService.CreateAsync(address);

            var rentalPlace = new RentalPlaceDto
            {
                LocationId = createdLocation.LocationId,
                AddressId = createdAddress.AddressId,
            };

            var createdRentalPlace = await base.CreateAsync(rentalPlace);

            // Change the location's RentalPlaceId to the created RentalPlaceId
            var locationEntity = await _locationsService.FindEntityById(createdLocation.LocationId);
            locationEntity.RentalPlaceId = createdRentalPlace.RentalPlaceId;
            await _locationsService.UpdateAsync(locationEntity.LocationId, _locationsService.MapSingleEntityToDto(locationEntity));

            return createdRentalPlace;
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

        public override RentalPlace MapToEntity(RentalPlaceDto model)
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
        public override Expression<Func<RentalPlace, RentalPlaceDto>> MapToDto()
        {
            return rp => new RentalPlaceDto
            {
                RentalPlaceId = rp.RentalPlaceId,
                LocationId = rp.LocationId,
                Gpslatitude = rp.Location.Gpslatitude,
                Gpslongitude = rp.Location.Gpslongitude,
                AddressId = rp.AddressId,
                CountryName = rp.Address.Country.Name,
                CountryId = rp.Address.CountryId,
                City = rp.Address.City,
                ZipCode = rp.Address.ZipCode,
                FirstLine = rp.Address.FirstLine,
                SecondLine = rp.Address.SecondLine,
                CreatedDate = rp.CreatedDate,
                ModifiedDate = rp.ModifiedDate,
                DeletedDate = rp.DeletedDate,
                IsActive = rp.IsActive,

                Address = rp.Address != null ? _addressesService.MapSingleEntityToDto(rp.Address) : null
            };
        }

        // Map a single entity to DTO
        public override RentalPlaceDto MapSingleEntityToDto(RentalPlace entity)
        {
            return new RentalPlaceDto
            {
                RentalPlaceId = entity.RentalPlaceId,
                LocationId = entity.LocationId,
                Gpslatitude = entity.Location.Gpslatitude,
                Gpslongitude = entity.Location.Gpslongitude,
                AddressId = entity.AddressId,
                CountryName = entity.Address.Country?.Name,
                CountryId = entity.Address.CountryId,
                City = entity.Address.City,
                FirstLine = entity.Address.FirstLine,
                SecondLine = entity.Address.SecondLine,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,

                Address = entity.Address != null ? _addressesService.MapSingleEntityToDto(entity.Address) : null
            };
        }

        // Find entity by ID
        public override async Task<RentalPlace> FindEntityById(int id)
        {
            return await _apiDbContext.RentalPlaces
                .Include(rp => rp.Location)
                .Include(rp => rp.Address)
                .Include(rp => rp.Address.Country)
                .FirstOrDefaultAsync(rp => rp.RentalPlaceId == id);
        }

        protected override IQueryable<RentalPlace> IncludeRelatedEntities(IQueryable<RentalPlace> query)
        {
            return query
                .Include(rp => rp.Location)
                .Include(rp => rp.Address)
                .Include(rp => rp.Address.Country);
        }

        // Update entity
        protected override void UpdateEntity(RentalPlace entity, RentalPlaceDto model)
        {
            entity.RentalPlaceId = model.RentalPlaceId;
            entity.LocationId = model.LocationId;
            entity.AddressId = model.AddressId;


            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }
        }

        public override async Task<RentalPlaceDto> UpdateAsync(int id, RentalPlaceDto updateDto)
        {
            var existingRentalPlace = await FindEntityById(id);

            // Update location
            var location = new LocationDto
            {
                LocationId = existingRentalPlace.LocationId,
                Gpslatitude = updateDto.Gpslatitude,
                Gpslongitude = updateDto.Gpslongitude,
                RentalPlaceId = id, // Maintain the relationship
                IsActive = updateDto.IsActive
            };
            await _locationsService.UpdateAsync(existingRentalPlace.LocationId, location);

            // Update address
            var address = new AddressDto
            {
                //AddressId = existingRentalPlace.AddressId,
                City = updateDto.City,
                FirstLine = updateDto.FirstLine,
                SecondLine = updateDto.SecondLine,
                ZipCode = updateDto.ZipCode,
                CountryId = updateDto.CountryId,
                IsActive = updateDto.IsActive,
                DeletedDate = updateDto.DeletedDate
            };
            await _addressesService.UpdateAsync(existingRentalPlace.AddressId, address);

            // Update rental place
            var rentalPlace = new RentalPlaceDto
            {
                RentalPlaceId = id,
                LocationId = existingRentalPlace.LocationId,
                AddressId = existingRentalPlace.AddressId,
                IsActive = updateDto.IsActive,
                DeletedDate = updateDto.DeletedDate
            };

            // Use base update to handle the rental place entity
            return await base.UpdateAsync(id, rentalPlace);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            // Get the rental place entity with its related IDs
            var rentalPlace = await FindEntityById(id);
            if (rentalPlace == null)
                return false;

            // Get locationId and addressId before deletion
            int locationId = rentalPlace.LocationId;
            int addressId = rentalPlace.AddressId;

            // Delete rental place using base implementation
            var rentalPlaceDeleted = await base.DeleteAsync(id);
            if (!rentalPlaceDeleted)
                return false;

            // Delete associated location and address
            await _locationsService.DeleteAsync(locationId);
            await _addressesService.DeleteAsync(addressId);

            return true;
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

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
            using var transaction = await _apiDbContext.Database.BeginTransactionAsync();
            try
            {
                // Set the tracking properties for the address and location
                createDto.Address.CreatedDate = DateTime.Now;
                createDto.Address.IsActive = true;
                createDto.Location.DateTime = DateTime.Now;
                createDto.Location.IsActive = true;

                var createdRentalPlace = await base.CreateAsync(createDto);

                // Change the location's RentalPlaceId to the created RentalPlaceId
                var locationEntity = await _locationsService.FindEntityById(createdRentalPlace.Location.LocationId);
                locationEntity.RentalPlaceId = createdRentalPlace.RentalPlaceId;
                await _locationsService.UpdateAsync(locationEntity.LocationId, _locationsService.MapSingleEntityToDto(locationEntity));

                await transaction.CommitAsync();
                return createdRentalPlace;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected override Expression<Func<RentalPlace, bool>> BuildSearchQuery(string search)
        {
            return rp =>
                rp.RentalPlaceId.ToString().Contains(search) ||
                rp.LocationId.ToString().Contains(search) ||
                rp.Location.Gpslatitude.ToString().Contains(search) ||
                rp.Location.Gpslongitude.ToString().Contains(search) ||
                rp.AddressId.ToString().Contains(search) ||
                rp.Address.City.Contains(search) ||
                rp.Address.FirstLine.Contains(search) ||
                (rp.Address.SecondLine != null && rp.Address.SecondLine.Contains(search)) ||
                rp.Address.ZipCode.Contains(search) ||
                rp.Address.Country.Name.Contains(search) ||
                rp.Address.Country.Abbreviation.Contains(search);
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
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                DeletedDate = model.DeletedDate,
                IsActive = model.IsActive,
                Address = _addressesService.MapToEntity(model.Address),
                Location = _locationsService.MapToEntity(model.Location)
            };
        }

        // Map Entity to DTO
        public override Expression<Func<RentalPlace, RentalPlaceDto>> MapToDto()
        {
            return rp => new RentalPlaceDto
            {
                RentalPlaceId = rp.RentalPlaceId,
                CreatedDate = rp.CreatedDate,
                ModifiedDate = rp.ModifiedDate,
                DeletedDate = rp.DeletedDate,
                IsActive = rp.IsActive,

                Address = rp.Address != null ? _addressesService.MapSingleEntityToDto(rp.Address) : null,
                Location = rp.Location != null ? _locationsService.MapSingleEntityToDto(rp.Location) : null
            };
        }

        // Map a single entity to DTO
        public override RentalPlaceDto MapSingleEntityToDto(RentalPlace entity)
        {
            return new RentalPlaceDto
            {
                RentalPlaceId = entity.RentalPlaceId,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,

                Address = entity.Address != null ? _addressesService.MapSingleEntityToDto(entity.Address) : null,
                Location = entity.Location != null ? _locationsService.MapSingleEntityToDto(entity.Location) : null
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

        protected override void UpdateEntity(RentalPlace entity, RentalPlaceDto model)
        {
            // Update RentalPlace core properties
            entity.IsActive = model.IsActive;
            entity.DeletedDate = model.IsActive ? null : model.DeletedDate;
            entity.ModifiedDate = DateTime.UtcNow;

            // Manual update because of the tracking
            // Update Address properties
            if (model.Address != null && entity.Address != null)
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

            // Update Location properties
            if (model.Location != null && entity.Location != null)
            {
                entity.Location.Gpslatitude = model.Location.Gpslatitude;
                entity.Location.Gpslongitude = model.Location.Gpslongitude;

                entity.Location.IsActive = model.IsActive;
                entity.Location.DateTime = DateTime.UtcNow;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            using var transaction = await _apiDbContext.Database.BeginTransactionAsync();
            try
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

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
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

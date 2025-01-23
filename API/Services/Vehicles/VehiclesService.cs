using API.Context;
using API.Models.DTOs.Vehicles;
using System.Linq.Expressions;
using API.Models;
using API.Models.DTOs.Other;
using API.Models.Other;
using API.Models.Vehicles;
using API.Services.Other;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Vehicles
{
    public class VehiclesService : BaseApiService<Vehicle, VehicleDto, VehicleDto>
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly VehicleOptionalInformationService _optionalInformationService;
        private readonly VehicleStatisticsService _statisticsService;
        private readonly LocationsService _locationsService;
        private readonly VehicleTypesService _vehicleTypesService;
        private readonly VehicleModelsService _vehicleModelsService;
        private readonly AddressesService _addressesService;
        private readonly RentalPlacesService _rentalPlacesService;

        public VehiclesService(
            ApiDbContext context,
            VehicleOptionalInformationService optionalInformationService,
            VehicleStatisticsService statisticsService,
            LocationsService locationsService,
            VehicleTypesService vehicleTypesService,
            VehicleModelsService vehicleModelsService,
            AddressesService addressesService,
            RentalPlacesService rentalPlacesService)
            : base(context)
        {
            _apiDbContext = context;
            _optionalInformationService = optionalInformationService;
            _statisticsService = statisticsService;
            _locationsService = locationsService;
            _vehicleTypesService = vehicleTypesService;
            _vehicleModelsService = vehicleModelsService;
            _addressesService = addressesService;
            _rentalPlacesService = rentalPlacesService;
        }

        public override async Task<VehicleDto> CreateAsync(VehicleDto createDto)
        {
            // Validate input
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            using var transaction = await _apiDbContext.Database.BeginTransactionAsync();
            try
            {
                // Create the related VehicleOptionalInformation, VehicleStatistics, Location first
                var vehicleOptionalInfoDto = new VehicleOptionalInformationDto
                {
                    HasNavigation = createDto.OptionalInformation?.HasNavigation ?? false,
                    HasBluetooth = createDto.OptionalInformation?.HasBluetooth ?? false,
                    HasAirConditioning = createDto.OptionalInformation?.HasAirConditioning ?? false,
                    HasAutomaticTransmission = createDto.OptionalInformation?.HasAutomaticTransmission ?? false,
                    HasParkingSensors = createDto.OptionalInformation?.HasParkingSensors ?? false,
                    HasCruiseControl = createDto.OptionalInformation?.HasCruiseControl ?? false
                };

                var vehicleStatisticsDto = new VehicleStatisticsDto
                {
                    TotalRentals = 0,
                    RentalRevenue = 0.0m,
                    FirstRentalDate = null,
                    LastRentalDate = null
                };

                var locationDto = new LocationDto
                {
                    Gpslatitude = 0.0,
                    Gpslongitude = 0.0
                };

                // Create the Optional Information and Statistics in the database
                var createdOptionalInformation =
                    await _optionalInformationService.CreateAsync(vehicleOptionalInfoDto);
                var createdStatistics = await _statisticsService.CreateAsync(vehicleStatisticsDto);
                var createdLocation = await _locationsService.CreateAsync(locationDto);

                // Now create the Vehicle entity and link the created Optional Information and Statistics
                var vehicle = new VehicleDto
                {
                    VehicleTypeId = createDto.VehicleTypeId,
                    VehicleModelId = createDto.VehicleModelId,
                    VehicleStatisticsId = createdStatistics.VehicleStatisticsId,
                    VehicleOptionalInformationId = createdOptionalInformation.VehicleOptionalInformationId,
                    RentalPlaceId = createDto.RentalPlaceId,
                    LocationId = createdLocation.LocationId,
                    Vin = createDto.Vin,
                    LicensePlate = createDto.LicensePlate,
                    Color = createDto.Color,
                    ManufactureYear = createDto.ManufactureYear,
                    CurrentMileage = createDto.CurrentMileage,
                    LastMaintenanceMileage = createDto.LastMaintenanceMileage,
                    LastMaintenanceDate = createDto.LastMaintenanceDate,
                    NextMaintenanceDate = createDto.NextMaintenanceDate,
                    PurchaseDate = createDto.PurchaseDate,
                    PurchasePrice = createDto.PurchasePrice,
                    Status = createDto.Status,
                    CustomDailyRate = createDto.CustomDailyRate,
                    CustomWeeklyRate = createDto.CustomWeeklyRate,
                    CustomDeposit = createDto.CustomDeposit,
                    IsAvailableForRent = createDto.IsAvailableForRent,
                    Notes = createDto.Notes,
                };

                var createdVehicle = await base.CreateAsync(vehicle);

                var locationEntity = await _locationsService.FindEntityById(createdLocation.LocationId);
                locationEntity.VehicleId = createdVehicle.VehicleId;
                await _locationsService.UpdateAsync(locationEntity.LocationId,
                    _locationsService.MapSingleEntityToDto(locationEntity));

                // Return the created Vehicle DTO
                await transaction.CommitAsync();
                return createdVehicle;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected override Expression<Func<Vehicle, bool>> BuildSearchQuery(string search)
        {
            // TODO: Implement search query

            return v =>
                v.VehicleId.ToString().Contains(search) ||
                v.VehicleModel.Name.Contains(search) ||
                (v.LicensePlate != null && v.LicensePlate.Contains(search)) ||
                v.Color.Contains(search) ||
                (v.Vin != null && v.Vin.Contains(search)) ||
                v.CurrentMileage.ToString().Contains(search) ||
                v.Status.Contains(search);
        }

        protected override Expression<Func<Vehicle, bool>> GetActiveFilter(bool showDeleted)
        {
            return v => v.IsActive != showDeleted;
        }

        #region Mapping

        public override Vehicle MapToEntity(VehicleDto dto)
        {
            return new Vehicle
            {
                VehicleId = dto.VehicleId,
                VehicleTypeId = dto.VehicleTypeId,
                VehicleModelId = dto.VehicleModelId,
                VehicleStatisticsId = dto.VehicleStatisticsId,
                VehicleOptionalInformationId = dto.VehicleOptionalInformationId,
                RentalPlaceId = dto.RentalPlaceId,
                LocationId = dto.LocationId,
                Vin = dto.Vin,
                LicensePlate = dto.LicensePlate,
                Color = dto.Color,
                ManufactureYear = dto.ManufactureYear,
                CurrentMileage = dto.CurrentMileage,
                LastMaintenanceMileage = dto.LastMaintenanceMileage,
                LastMaintenanceDate = dto.LastMaintenanceDate,
                NextMaintenanceDate = dto.NextMaintenanceDate,
                PurchaseDate = dto.PurchaseDate,
                PurchasePrice = dto.PurchasePrice,
                Status = dto.Status,
                CustomDailyRate = dto.CustomDailyRate,
                CustomWeeklyRate = dto.CustomWeeklyRate,
                CustomDeposit = dto.CustomDeposit,
                IsAvailableForRent = dto.IsAvailableForRent,
                Notes = dto.Notes,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            };
        }

        public override Expression<Func<Vehicle, VehicleDto>> MapToDto()
        {
            return v => new VehicleDto
            {
                VehicleId = v.VehicleId,
                VehicleTypeId = v.VehicleTypeId,
                VehicleModelId = v.VehicleModelId,
                VehicleStatisticsId = v.VehicleStatisticsId,
                VehicleOptionalInformationId = v.VehicleOptionalInformationId,
                RentalPlaceId = v.RentalPlaceId,
                LocationId = v.LocationId,
                Vin = v.Vin,
                LicensePlate = v.LicensePlate,
                Color = v.Color,
                ManufactureYear = v.ManufactureYear,
                CurrentMileage = v.CurrentMileage,
                LastMaintenanceMileage = v.LastMaintenanceMileage,
                LastMaintenanceDate = v.LastMaintenanceDate,
                NextMaintenanceDate = v.NextMaintenanceDate,
                PurchaseDate = v.PurchaseDate,
                PurchasePrice = v.PurchasePrice,
                Status = v.Status,
                CustomDailyRate = v.CustomDailyRate,
                CustomWeeklyRate = v.CustomWeeklyRate,
                CustomDeposit = v.CustomDeposit,
                IsAvailableForRent = v.IsAvailableForRent,
                Notes = v.Notes,
                CreatedDate = v.CreatedDate,
                IsActive = v.IsActive,
                VehicleType = MapVehicleTypeToDto(v.VehicleType),
                VehicleModel = MapVehicleModelToDto(v.VehicleModel),
                RentalPlace = _rentalPlacesService.MapSingleEntityToDto(v.RentalPlace),
                VehicleStatistics = MapVehicleStatisticsToDto(v.VehicleStatistics),
                OptionalInformation = MapVehicleOptionalInformationToDto(v.VehicleOptionalInformation)
            };
        }

        private static VehicleTypeDto MapVehicleTypeToDto(VehicleType vehicleType)
        {
            return new VehicleTypeDto
            {
                VehicleTypeId = vehicleType.VehicleTypeId,
                Name = vehicleType.Name
            };
        }

        private static VehicleModelDto MapVehicleModelToDto(VehicleModel vehicleModel)
        {
            return new VehicleModelDto
            {
                VehicleModelId = vehicleModel.VehicleModelId,
                Name = vehicleModel.Name,
                VehicleBrandName = vehicleModel.VehicleBrand?.Name
            };
        }

        private static RentalPlaceDto MapRentalPlaceToDto(RentalPlace rentalPlace)
        {
            return new RentalPlaceDto
            {
                RentalPlaceId = rentalPlace.RentalPlaceId,
                CountryName = rentalPlace.Address?.Country?.Name,
                City = rentalPlace.Address?.City,
            };
        }

        private static VehicleStatisticsDto MapVehicleStatisticsToDto(VehicleStatistic vehicleStatistics)
        {
            return new VehicleStatisticsDto
            {
                VehicleStatisticsId = vehicleStatistics.VehicleStatisticsId,
                TotalRentals = vehicleStatistics.TotalRentals,
                RentalRevenue = vehicleStatistics.RentalRevenue,
                FirstRentalDate = vehicleStatistics.FirstRentalDate,
                LastRentalDate = vehicleStatistics.LastRentalDate
            };
        }

        private static VehicleOptionalInformationDto MapVehicleOptionalInformationToDto(VehicleOptionalInformation optionalInformation)
        {
            return new VehicleOptionalInformationDto
            {
                VehicleOptionalInformationId = optionalInformation.VehicleOptionalInformationId,
                HasNavigation = optionalInformation.HasNavigation,
                HasBluetooth = optionalInformation.HasBluetooth,
                HasAirConditioning = optionalInformation.HasAirConditioning,
                HasAutomaticTransmission = optionalInformation.HasAutomaticTransmission,
                HasParkingSensors = optionalInformation.HasParkingSensors,
                HasCruiseControl = optionalInformation.HasCruiseControl
            };
        }

        public override VehicleDto MapSingleEntityToDto(Vehicle entity)
        {
            return new VehicleDto
            {
                VehicleId = entity.VehicleId,
                VehicleTypeId = entity.VehicleTypeId,
                VehicleModelId = entity.VehicleModelId,
                VehicleStatisticsId = entity.VehicleStatisticsId,
                VehicleOptionalInformationId = entity.VehicleOptionalInformationId,
                RentalPlaceId = entity.RentalPlaceId,
                LocationId = entity.LocationId,
                Vin = entity.Vin,
                LicensePlate = entity.LicensePlate,
                Color = entity.Color,
                ManufactureYear = entity.ManufactureYear,
                CurrentMileage = entity.CurrentMileage,
                LastMaintenanceMileage = entity.LastMaintenanceMileage,
                LastMaintenanceDate = entity.LastMaintenanceDate,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                PurchaseDate = entity.PurchaseDate,
                PurchasePrice = entity.PurchasePrice,
                Status = entity.Status,
                CustomDailyRate = entity.CustomDailyRate,
                CustomWeeklyRate = entity.CustomWeeklyRate,
                CustomDeposit = entity.CustomDeposit,
                IsAvailableForRent = entity.IsAvailableForRent,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                DeletedDate = entity.DeletedDate,
                IsActive = entity.IsActive,
                VehicleType = entity.VehicleType != null ? new VehicleTypeDto
                {
                    VehicleTypeId = entity.VehicleType.VehicleTypeId,
                    Name = entity.VehicleType.Name
                } : null,
                VehicleModel = entity.VehicleModel != null ? new VehicleModelDto
                {
                    VehicleModelId = entity.VehicleModel.VehicleModelId,
                    Name = entity.VehicleModel.Name
                } : null,
                RentalPlace = entity.RentalPlace != null ? new RentalPlaceDto
                {
                    RentalPlaceId = entity.RentalPlace.RentalPlaceId,
                    CountryName = entity.RentalPlace.Address.Country.Name,
                    City = entity.RentalPlace.Address.City,
                    Address = _addressesService.MapSingleEntityToDto(entity.RentalPlace.Address)
                } : null,
                VehicleStatistics = entity.VehicleStatistics != null ? new VehicleStatisticsDto
                {
                    VehicleStatisticsId = entity.VehicleStatistics.VehicleStatisticsId,
                    TotalRentals = entity.VehicleStatistics.TotalRentals,
                    RentalRevenue = entity.VehicleStatistics.RentalRevenue,
                    FirstRentalDate = entity.VehicleStatistics.FirstRentalDate,
                    LastRentalDate = entity.VehicleStatistics.LastRentalDate
                } : null,
                OptionalInformation = entity.VehicleOptionalInformation != null ? new VehicleOptionalInformationDto
                {
                    VehicleOptionalInformationId = entity.VehicleOptionalInformation.VehicleOptionalInformationId,
                    HasNavigation = entity.VehicleOptionalInformation.HasNavigation,
                    HasBluetooth = entity.VehicleOptionalInformation.HasBluetooth,
                    HasAirConditioning = entity.VehicleOptionalInformation.HasAirConditioning,
                    HasAutomaticTransmission = entity.VehicleOptionalInformation.HasAutomaticTransmission,
                    HasParkingSensors = entity.VehicleOptionalInformation.HasParkingSensors,
                    HasCruiseControl = entity.VehicleOptionalInformation.HasCruiseControl
                } : null
            };
        }

        #endregion

        public override async Task<VehicleDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(Vehicle).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(Vehicle entity, VehicleDto model)
        {
            entity.VehicleTypeId = model.VehicleTypeId;
            entity.VehicleModelId = model.VehicleModelId;
            entity.RentalPlaceId = model.RentalPlaceId;
            entity.Vin = model.Vin;
            entity.LicensePlate = model.LicensePlate;
            entity.Color = model.Color;
            entity.ManufactureYear = model.ManufactureYear;
            entity.CurrentMileage = model.CurrentMileage;
            entity.LastMaintenanceMileage = model.LastMaintenanceMileage;
            entity.LastMaintenanceDate = model.LastMaintenanceDate;
            entity.NextMaintenanceDate = model.NextMaintenanceDate;
            entity.PurchaseDate = model.PurchaseDate;
            entity.PurchasePrice = model.PurchasePrice;
            entity.Status = model.Status;
            entity.CustomDailyRate = model.CustomDailyRate;
            entity.CustomWeeklyRate = model.CustomWeeklyRate;
            entity.CustomDeposit = model.CustomDeposit;
            entity.IsAvailableForRent = model.IsAvailableForRent;
            entity.Notes = model.Notes;

            if (model.IsActive)
            {
                entity.DeletedDate = model.DeletedDate;
                entity.IsActive = model.IsActive;
            }

            // Update vehicle optional information
            _optionalInformationService.UpdateAsync(entity.VehicleOptionalInformationId, model.OptionalInformation);
        }

        public override async Task<Vehicle> FindEntityById(int id)
        {
            return await _apiDbContext.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.RentalPlace)
                .Include(v => v.RentalPlace.Address)
                .Include(v => v.RentalPlace.Address.Country)
                .Include(v => v.VehicleStatistics)
                .Include(v => v.VehicleOptionalInformation)
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        protected override IQueryable<Vehicle> IncludeRelatedEntities(IQueryable<Vehicle> query)
        {
            return query
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.RentalPlace)
                .Include(v => v.RentalPlace.Address)
                .Include(v => v.RentalPlace.Address.Country)
                .Include(v => v.VehicleStatistics)
                .Include(v => v.VehicleOptionalInformation);
        }
    }
}   

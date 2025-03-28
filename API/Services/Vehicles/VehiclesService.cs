﻿using System.Linq.Expressions;
using API.Context;
using API.Models;
using API.Models.DTOs.Other;
using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using API.Services.Other;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.Vehicles;

public class VehiclesService : BaseApiService<Vehicle, VehicleDto, VehicleDto>
{
    private readonly AddressesService _addressesService;
    private readonly ApiDbContext _apiDbContext;
    private readonly LocationsService _locationsService;
    private readonly VehicleOptionalInformationService _optionalInformationService;
    private readonly RentalPlacesService _rentalPlacesService;
    private readonly VehicleStatisticsService _statisticsService;
    private readonly VehicleModelsService _vehicleModelsService;
    private readonly VehicleTypesService _vehicleTypesService;
    private readonly VehicleStatusesService _vehicleStatusService;

    public VehiclesService(
        ApiDbContext context,
        VehicleOptionalInformationService optionalInformationService,
        VehicleStatisticsService statisticsService,
        LocationsService locationsService,
        VehicleTypesService vehicleTypesService,
        VehicleModelsService vehicleModelsService,
        AddressesService addressesService,
        RentalPlacesService rentalPlacesService,
        VehicleStatusesService vehicleStatusService)
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
        _vehicleStatusService = vehicleStatusService;
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
                HasNavigation = createDto.VehicleOptionalInformation.HasNavigation,
                HasBluetooth = createDto.VehicleOptionalInformation.HasBluetooth,
                HasAirConditioning = createDto.VehicleOptionalInformation.HasAirConditioning,
                HasAutomaticTransmission = createDto.VehicleOptionalInformation.HasAutomaticTransmission,
                HasParkingSensors = createDto.VehicleOptionalInformation.HasParkingSensors,
                HasCruiseControl = createDto.VehicleOptionalInformation.HasCruiseControl
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
                VehicleTypeId = createDto.VehicleType.VehicleTypeId,
                VehicleModelId = createDto.VehicleModel.VehicleModelId,
                VehicleStatisticsId = createdStatistics.VehicleStatisticsId,
                VehicleOptionalInformationId = createdOptionalInformation.VehicleOptionalInformationId,
                RentalPlaceId = createDto.RentalPlace.RentalPlaceId,
                LocationId = createdLocation.LocationId,
                VehicleStatusId = createDto.VehicleStatus.VehicleStatusId,

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

                // If custom rates are not provided, use the default rates from Vehicle Type
                CustomDailyRate = createDto.CustomDailyRate ?? createDto.VehicleType.BaseDailyRate,
                CustomWeeklyRate = createDto.CustomWeeklyRate ?? createDto.VehicleType.BaseWeeklyRate,
                CustomDeposit = createDto.CustomDeposit ?? createDto.VehicleType.BaseDeposit,

                IsAvailableForRent = createDto.IsAvailableForRent,
                Notes = createDto.Notes
            };

            var createdVehicle = await base.CreateAsync(vehicle);

            var locationEntity = await _locationsService.FindEntityById(createdVehicle.LocationId);
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

    public override async Task<bool> DeleteAsync(int id)
    {
        using var transaction = await _apiDbContext.Database.BeginTransactionAsync();
        try
        {
            var vehicle = await FindEntityById(id);
            if (vehicle == null) throw new KeyNotFoundException($"{typeof(Vehicle).Name} not found");

            // Delete location
            var location = await _locationsService.FindEntityById(vehicle.LocationId);
            if (location != null)
            {
                await _locationsService.DeleteAsync(location.LocationId);
            }

            var vehicleDeleted = await base.DeleteAsync(id);
            if (!vehicleDeleted) return false;

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PaginatedResult<VehicleDto>> GetVehiclesMaintenanceAsync(
        string? search = null,
        int page = 1,
        int pageSize = 10,
        DateTime? createdBefore = null,
        DateTime? createdAfter = null,
        DateTime? modifiedBefore = null,
        DateTime? modifiedAfter = null)
    {
        var query = _apiDbContext.Vehicles
            .Where(r => r.VehicleStatusId == 3 && r.IsActive);

        return await GetAllAsync(
            search, page, false, createdBefore, createdAfter, modifiedBefore, modifiedAfter, pageSize, query);
    }

    protected override Expression<Func<Vehicle, bool>> BuildSearchQuery(string search)
    {
        // TODO: Implement search query

        return v =>
            v.VehicleId.ToString().Contains(search) ||
            (v.VehicleModel != null && v.VehicleModel.Name != null && v.VehicleModel.Name.Contains(search)) ||
            (v.VehicleModel != null && v.VehicleModel.VehicleBrand != null &&
             v.VehicleModel.VehicleBrand.Name != null && v.VehicleModel.VehicleBrand.Name.Contains(search)) ||
            (v.VehicleType != null && v.VehicleType.Name != null && v.VehicleType.Name.Contains(search)) ||
            (v.LicensePlate != null && v.LicensePlate.Contains(search)) ||
            (v.Color != null && v.Color.Contains(search)) ||
            (v.Vin != null && v.Vin.Contains(search)) ||
            v.CurrentMileage.ToString().Contains(search) ||
            v.ManufactureYear.ToString().Contains(search) ||
            (v.LastMaintenanceMileage != null && v.LastMaintenanceMileage.ToString().Contains(search)) ||
            (v.LastMaintenanceDate != null && v.LastMaintenanceDate.Value.ToString().Contains(search)) ||
            (v.NextMaintenanceDate != null && v.NextMaintenanceDate.Value.ToString().Contains(search)) ||
            v.PurchaseDate.ToString().Contains(search) ||
            v.PurchasePrice.ToString().Contains(search) ||
            v.VehicleStatus.StatusName.Contains(search) ||
            (v.CustomDailyRate != null && v.CustomDailyRate.Value.ToString().Contains(search)) ||
            (v.CustomWeeklyRate != null && v.CustomWeeklyRate.Value.ToString().Contains(search)) ||
            (v.CustomDeposit != null && v.CustomDeposit.Value.ToString().Contains(search)) ||
            (v.Notes != null && v.Notes.Contains(search)) ||
            (v.RentalPlace != null && v.RentalPlace.Address.City != null &&
             v.RentalPlace.Address.City.Contains(search)) ||
            (v.RentalPlace != null && v.RentalPlace.Address.Country.Name != null) &&
            v.RentalPlace.Address.Country.Name.Contains(search) ||
            v.IsAvailableForRent.ToString().Contains(search);
    }

    protected override Expression<Func<Vehicle, bool>> GetActiveFilter(bool showDeleted)
    {
        return v => v.IsActive != showDeleted;
    }

    public override async Task<VehicleDto> GetByIdAsync(int id)
    {
        var entity = await FindEntityById(id);
        if (entity == null) throw new KeyNotFoundException($"{typeof(Vehicle).Name} not found");

        return MapSingleEntityToDto(entity);
    }

    protected override void UpdateEntity(Vehicle entity, VehicleDto model)
    {
        entity.VehicleTypeId = model.VehicleType.VehicleTypeId;
        entity.VehicleModelId = model.VehicleModel.VehicleModelId;
        entity.RentalPlaceId = model.RentalPlace.RentalPlaceId;
        entity.VehicleStatusId = model.VehicleStatus.VehicleStatusId;
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
        entity.IsAvailableForRent = model.IsAvailableForRent;
        entity.Notes = model.Notes;

        if (model.VehicleOptionalInformation != null)
        {
            entity.VehicleOptionalInformation.HasAirConditioning = model.VehicleOptionalInformation.HasAirConditioning;
            entity.VehicleOptionalInformation.HasAutomaticTransmission = model.VehicleOptionalInformation.HasAutomaticTransmission;
            entity.VehicleOptionalInformation.HasBluetooth = model.VehicleOptionalInformation.HasBluetooth;
            entity.VehicleOptionalInformation.HasCruiseControl = model.VehicleOptionalInformation.HasCruiseControl;
            entity.VehicleOptionalInformation.HasNavigation = model.VehicleOptionalInformation.HasNavigation;
            entity.VehicleOptionalInformation.HasParkingSensors = model.VehicleOptionalInformation.HasParkingSensors;
        }

        // entity.Location.IsActive = model.IsActive;

        // If custom rates are not provided, use the default rates from Vehicle Type
        if (model.VehicleType != null)
        {
            entity.CustomDailyRate = model.CustomDailyRate ?? model.VehicleType.BaseDailyRate;
            entity.CustomWeeklyRate = model.CustomWeeklyRate ?? model.VehicleType.BaseWeeklyRate;
            entity.CustomDeposit = model.CustomDeposit ?? model.VehicleType.BaseDeposit;
        }

        // Restore the vehicle to active state if it was previously deleted
        if (model.IsActive)
        {
            entity.DeletedDate = model.DeletedDate;
            entity.IsActive = model.IsActive;
            entity.Location.DateTime = DateTime.Now;
            entity.Location.IsActive = model.IsActive;
        }
    }

    public override async Task<Vehicle> FindEntityById(int id)
    {
        return await _apiDbContext.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleModel.VehicleBrand)
            .Include(v => v.VehicleStatus)
            .Include(v => v.RentalPlace)
            .Include(v => v.RentalPlace.Address)
            .Include(v => v.RentalPlace.Address.Country)
            .Include(v => v.VehicleStatistics)
            .Include(v => v.VehicleOptionalInformation)
            .Include(v => v.Location)
            .FirstOrDefaultAsync(v => v.VehicleId == id);
    }

    protected override IQueryable<Vehicle> IncludeRelatedEntities(IQueryable<Vehicle> query)
    {
        return query
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleModel.VehicleBrand)
            .Include(v => v.VehicleStatus)
            .Include(v => v.RentalPlace)
            .Include(v => v.RentalPlace.Address)
            .Include(v => v.RentalPlace.Address.Country)
            .Include(v => v.VehicleStatistics)
            .Include(v => v.VehicleOptionalInformation)
            .Include(v => v.Location);
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
            VehicleStatusId = dto.VehicleStatusId,
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
            VehicleStatusId = v.VehicleStatusId,
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
            CustomDailyRate = v.CustomDailyRate,
            CustomWeeklyRate = v.CustomWeeklyRate,
            CustomDeposit = v.CustomDeposit,
            IsAvailableForRent = v.IsAvailableForRent,
            Notes = v.Notes,
            CreatedDate = v.CreatedDate,
            IsActive = v.IsActive,
            VehicleType = v.VehicleType != null ? _vehicleTypesService.MapSingleEntityToDto(v.VehicleType) : null,
            VehicleModel = v.VehicleModel != null ? _vehicleModelsService.MapSingleEntityToDto(v.VehicleModel) : null,
            RentalPlace = v.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(v.RentalPlace) : null,
            VehicleStatus = v.VehicleStatus != null ? _vehicleStatusService.MapSingleEntityToDto(v.VehicleStatus) : null,
            VehicleStatistics = v.VehicleStatistics != null ? _statisticsService.MapSingleEntityToDto(v.VehicleStatistics) : null,
            VehicleOptionalInformation = v.VehicleOptionalInformation != null
                ? _optionalInformationService.MapSingleEntityToDto(v.VehicleOptionalInformation)
                : null,
            Location = v.Location != null ? _locationsService.MapSingleEntityToDto(v.Location) : null
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
            CustomDailyRate = entity.CustomDailyRate,
            CustomWeeklyRate = entity.CustomWeeklyRate,
            CustomDeposit = entity.CustomDeposit,
            IsAvailableForRent = entity.IsAvailableForRent,
            Notes = entity.Notes,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            DeletedDate = entity.DeletedDate,
            IsActive = entity.IsActive,
            VehicleType = entity.VehicleType != null ? _vehicleTypesService.MapSingleEntityToDto(entity.VehicleType) : null,
            VehicleModel = entity.VehicleModel != null ? _vehicleModelsService.MapSingleEntityToDto(entity.VehicleModel) : null,
            RentalPlace = entity.RentalPlace != null ? _rentalPlacesService.MapSingleEntityToDto(entity.RentalPlace) : null,
            VehicleStatus = entity.VehicleStatus != null ? _vehicleStatusService.MapSingleEntityToDto(entity.VehicleStatus) : null,
            VehicleStatistics = entity.VehicleStatistics != null ? _statisticsService.MapSingleEntityToDto(entity.VehicleStatistics) : null,
            VehicleOptionalInformation = entity.VehicleOptionalInformation != null
                ? _optionalInformationService.MapSingleEntityToDto(entity.VehicleOptionalInformation)
                : null,
            Location = entity.Location != null ? _locationsService.MapSingleEntityToDto(entity.Location) : null
        };
    }

    #endregion
}
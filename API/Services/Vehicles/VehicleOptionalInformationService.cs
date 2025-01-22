using API.Context;
using System.Linq.Expressions;
using API.Models.Vehicles;
using API.Models.DTOs.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Vehicles
{
    public class VehicleOptionalInformationService : BaseApiService<VehicleOptionalInformation, VehicleOptionalInformationDto, VehicleOptionalInformationDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public VehicleOptionalInformationService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<VehicleOptionalInformation, bool>> BuildSearchQuery(string search)
        {
            return voi =>
                voi.VehicleOptionalInformationId.ToString().Contains(search) ||
                voi.HasNavigation.ToString().Contains(search) ||
                voi.HasBluetooth.ToString().Contains(search) ||
                voi.HasAirConditioning.ToString().Contains(search) ||
                voi.HasAutomaticTransmission.ToString().Contains(search) ||
                voi.HasParkingSensors.ToString().Contains(search) ||
                voi.HasCruiseControl.ToString().Contains(search);
        }

        protected override Expression<Func<VehicleOptionalInformation, bool>> GetActiveFilter(bool showDeleted)
        {
            return voi => true;
        }

        public override VehicleOptionalInformation MapToEntity(VehicleOptionalInformationDto model)
        {
            return new VehicleOptionalInformation
            {
                VehicleOptionalInformationId = model.VehicleOptionalInformationId,
                HasNavigation = model.HasNavigation,
                HasBluetooth = model.HasBluetooth,
                HasAirConditioning = model.HasAirConditioning,
                HasAutomaticTransmission = model.HasAutomaticTransmission,
                HasParkingSensors = model.HasParkingSensors,
                HasCruiseControl = model.HasCruiseControl
            };
        }

        public override Expression<Func<VehicleOptionalInformation, VehicleOptionalInformationDto>> MapToDto()
        {
            return voi => new VehicleOptionalInformationDto
            {
                VehicleOptionalInformationId = voi.VehicleOptionalInformationId,
                HasNavigation = voi.HasNavigation,
                HasBluetooth = voi.HasBluetooth,
                HasAirConditioning = voi.HasAirConditioning,
                HasAutomaticTransmission = voi.HasAutomaticTransmission,
                HasParkingSensors = voi.HasParkingSensors,
                HasCruiseControl = voi.HasCruiseControl
            };
        }

        public override VehicleOptionalInformationDto MapSingleEntityToDto(VehicleOptionalInformation entity)
        {
            return new VehicleOptionalInformationDto
            {
                VehicleOptionalInformationId = entity.VehicleOptionalInformationId,
                HasNavigation = entity.HasNavigation,
                HasBluetooth = entity.HasBluetooth,
                HasAirConditioning = entity.HasAirConditioning,
                HasAutomaticTransmission = entity.HasAutomaticTransmission,
                HasParkingSensors = entity.HasParkingSensors,
                HasCruiseControl = entity.HasCruiseControl
            };
        }

        public override async Task<VehicleOptionalInformationDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleOptionalInformation).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleOptionalInformation entity, VehicleOptionalInformationDto model)
        {
            entity.HasNavigation = model.HasNavigation;
            entity.HasBluetooth = model.HasBluetooth;
            entity.HasAirConditioning = model.HasAirConditioning;
            entity.HasAutomaticTransmission = model.HasAutomaticTransmission;
            entity.HasParkingSensors = model.HasParkingSensors;
            entity.HasCruiseControl = model.HasCruiseControl;
        }

        public override async Task<VehicleOptionalInformation> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleOptionalInformations
                .FirstOrDefaultAsync(voi => voi.VehicleOptionalInformationId == id);
        }
    }
}

using API.Context;
using API.Models.DTOs.Vehicles;
using API.Models.Vehicles;
using System.Linq.Expressions;

namespace API.Services.Vehicles
{
    public class VehicleStatusesService : BaseApiService<VehicleStatus, VehicleStatusDto, VehicleStatusDto>
    {
        private readonly ApiDbContext _apiDbContext;

        public VehicleStatusesService(ApiDbContext context) : base(context)
        {
            _apiDbContext = context;
        }

        protected override Expression<Func<VehicleStatus, bool>> BuildSearchQuery(string search)
        {
            return vs =>
                vs.VehicleStatusId.ToString().Contains(search) ||
                vs.StatusName.Contains(search) ||
                vs.Description.Contains(search);
        }

        #region Mapping

        public override VehicleStatus MapToEntity(VehicleStatusDto model)
        {
            return new VehicleStatus
            {
                VehicleStatusId = model.VehicleStatusId,
                StatusName = model.StatusName,
                Description = model.Description
            };
        }

        public override Expression<Func<VehicleStatus, VehicleStatusDto>> MapToDto()
        {
            return vs => new VehicleStatusDto
            {
                VehicleStatusId = vs.VehicleStatusId,
                StatusName = vs.StatusName,
                Description = vs.Description
            };
        }

        public override VehicleStatusDto MapSingleEntityToDto(VehicleStatus entity)
        {
            return new VehicleStatusDto
            {
                VehicleStatusId = entity.VehicleStatusId,
                StatusName = entity.StatusName,
                Description = entity.Description
            };
        }

        #endregion

        public override async Task<VehicleStatusDto> GetByIdAsync(int id)
        {
            var entity = await FindEntityById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(VehicleStatus).Name} not found");
            }

            return MapSingleEntityToDto(entity);
        }

        protected override void UpdateEntity(VehicleStatus entity, VehicleStatusDto model)
        {
            entity.StatusName = model.StatusName;
            entity.Description = model.Description;
        }

        public override async Task<VehicleStatus> FindEntityById(int id)
        {
            return await _apiDbContext.VehicleStatuses.FindAsync(id);
        }
    }
}

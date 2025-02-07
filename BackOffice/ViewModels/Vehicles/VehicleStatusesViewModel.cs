using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Helpers;
using BackOffice.Models.DTOs.Vehicles;

namespace BackOffice.ViewModels.Vehicles
{
    public class VehicleStatusesViewModel : BaseListViewModel<VehicleStatusDto>
    {
        public VehicleStatusesViewModel() : base("VehicleStatuses", LocalizationHelper.GetString("VehicleStatuses", "DisplayName"))
        {
            
        }
    }
}

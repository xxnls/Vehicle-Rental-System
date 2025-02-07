using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleStatusDto : BaseDtoModel
    {
        public int VehicleStatusId { get; set; }

        private string _statusName = string.Empty;
        public string StatusName
        {
            get => _statusName;
            set
            {
                _statusName = value;
                OnPropertyChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }
}

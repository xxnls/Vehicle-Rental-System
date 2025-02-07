namespace API.Models.Vehicles
{
    public partial class VehicleStatus
    {
        public int VehicleStatusId { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}

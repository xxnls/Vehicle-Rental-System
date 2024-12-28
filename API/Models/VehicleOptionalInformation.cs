using System;
using System.Collections.Generic;

namespace API.Models;

public partial class VehicleOptionalInformation
{
    public int VehicleOptionalInformationId { get; set; }

    public bool HasNavigation { get; set; }

    public bool HasBluetooth { get; set; }

    public bool HasAirConditioning { get; set; }

    public bool HasAutomaticTransmission { get; set; }

    public bool HasParkingSensors { get; set; }

    public bool HasCruiseControl { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

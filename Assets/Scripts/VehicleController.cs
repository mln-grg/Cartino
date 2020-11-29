using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController
{
    public VehicleController(VehicleModel vehicleModel , VehicleView vehiclePrefab)
    {
        VehicleModel = vehicleModel;
        VehicleView = GameObject.Instantiate<VehicleView>(vehiclePrefab);
    }
    public VehicleView VehicleView { get; }
    public VehicleModel VehicleModel { get; }

}

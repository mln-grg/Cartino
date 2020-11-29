using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleModel : MonoSingletonGeneric<VehicleModel>
{
    public VehicleModel(float acceleration , float health)
    {
        Acceleration = acceleration;
        Health = health;
    }
    public float Health { get; }
    public float Acceleration { get; }
}

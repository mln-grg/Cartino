using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleService : MonoBehaviour
{
    [SerializeField] private VehicleView vehiclePrefab;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        VehicleModel model = new VehicleModel(8, 100f);
        VehicleController vehicle = new VehicleController(model, vehiclePrefab);
      
    }
}

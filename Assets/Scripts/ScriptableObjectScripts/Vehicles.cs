using UnityEngine;

[CreateAssetMenu(fileName = "New Vehicle", menuName = "Vehicles")]
public class Vehicles : ScriptableObject
{
    public GameObject CarPrefab;
    public GameObject explosionPrefab;
    public float Health;
    public float acceleration;
    public float turnSpeed;

}

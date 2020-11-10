using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smooth = 0.3f;
    //[SerializeField] private float height;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        Vector3 pos = new Vector3();
        pos.x = player.position.x + offset.x;
        pos.z = player.position.z - offset.z;
        pos.y = player.position.y + offset.y;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
    }
}

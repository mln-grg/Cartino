using UnityEngine;

public class CameraController : MonoBehaviour
{
    //    [SerializeField] private Vector3 offset;
    //    [SerializeField] private Transform target;
    //    [SerializeField] private float translateSpeed;
    //    [SerializeField] private float rotationSpeed;

    //    private void FixedUpdate()
    //    {
    //        HandleTranslation();
    //        HandleRotation();
    //    }

    //    private void HandleTranslation()
    //    {
    //        var targetPosition = target.TransformPoint(offset);
    //        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    //    }
    //    private void HandleRotation()
    //    {
    //        var direction = target.position - transform.position;
    //        var rotation = Quaternion.LookRotation(direction, Vector3.up);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    //    }

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

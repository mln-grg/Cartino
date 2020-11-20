using UnityEngine;

public class ColliderRotationFix : MonoBehaviour
{
    public BoxCollider col;
    private Quaternion rotbackup;
    private Transform tbackup;
    private void Start()
    {
        //col.transform.parent = null;
        rotbackup = col.transform.localRotation;
        tbackup.position = col.transform.localPosition;
    }

    private void Update()
    {
        
        col.transform.localPosition = tbackup.position;
        col.transform.localRotation = rotbackup;
    }
}

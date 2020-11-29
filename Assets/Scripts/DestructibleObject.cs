using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour,Destructible
{
    [SerializeField] private GameObject DestroyedObj;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Shell" || collision.gameObject.tag == "PlayerSphere")
        {
            IDestructible();
        }
    }
    public void IDestructible()
    {
        GameObject obj = Instantiate(DestroyedObj, transform.position + offset, Quaternion.identity);
        Rigidbody[] rgbds = obj.GetComponentsInChildren<Rigidbody>();

        if (rgbds.Length > 0)
        {
            foreach(var rb in rgbds)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(this.gameObject);
    }
}

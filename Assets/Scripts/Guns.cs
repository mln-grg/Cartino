using System;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject HitImpact;


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            muzzleFlash.Play();
            Shoot();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            muzzleFlash.Stop();
        }
    }

    private void Shoot()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            //if(hit.transform.tag == "Destructible")
            {
                //DestructibleObject dO = hit.transform.GetComponent<DestructibleObject>();
                //dO.IDestructible();
                //hit.transform.GetComponent<DestructibleObject>().IDestructible();
            }
            EnemyController target =hit.transform.GetComponent<EnemyController>();
            if (target != null)
            {
                target.TakeDamage(damage);
                //hit.transform.GetComponent<EnemyDamage>().Pow(hit.transform.position);

            }
            GameObject impact = Instantiate(HitImpact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);

        }
    }
}

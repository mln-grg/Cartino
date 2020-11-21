using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShooting : MonoBehaviour
{
    [SerializeField] private Rigidbody shell;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float minLaunchForce = 15f;
    [SerializeField] private float maxLaunchForce = 30f;
    [SerializeField] private float maxChargeTime = 0.75f;

    private float currentLaunchForce;
    private float chargeSpeed;
    private bool fired;

    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
    }   
    private void Start()
    {
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }

    private void Update()
    {
        if(currentLaunchForce >= maxLaunchForce && !fired)
        {
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            fired = false;
            currentLaunchForce = minLaunchForce;
        }
        else if (Input.GetMouseButton(1) && !fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(1) && !fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;
        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        shellInstance.velocity = currentLaunchForce * fireTransform.forward;
        currentLaunchForce = minLaunchForce;
    }
}

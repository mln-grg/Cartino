using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShooting : MonoBehaviour,IpooledObject
{
    //[SerializeField] private Rigidbody shell;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float minLaunchForce = 15f;
    [SerializeField] private float maxLaunchForce = 30f;
    [SerializeField] private float maxChargeTime = 0.75f;
    //[SerializeField] private Button fireB;

    private float currentLaunchForce;
    private float chargeSpeed;
    private bool fired;
    private ShellPooler shellPooler;
    [SerializeField] private ParticleSystem mortarMuzzle;
    private PlayerHealth ph;

    [SerializeField] GameObject bomb;
    public AudioSource shootingSound;
    public AudioSource bombShootingSound;
    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
    }   
    private void Start()
    {
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        shellPooler = ShellPooler.GetInstance();
        ph = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (ph.IsDead == false)
        {
            if (currentLaunchForce >= maxLaunchForce && !fired)
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
            else if (Input.GetMouseButton(2) &&!fired)
            {
                Bomb();
            }
            else if (Input.GetMouseButtonUp(2))
            {
                fired = false;
            }
            
        }
    }

    public void Fire()
    {
        EventTriggers.instance.FiredShells();
        PlayerController.Instance.ForwardPush();
        shootingSound.Play();
        fired = true;
        OnObjectSpawn();
        mortarMuzzle.Play();
        //Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
        //shellInstance.velocity = currentLaunchForce * fireTransform.forward;
        currentLaunchForce = minLaunchForce;
    }
    public void Bomb()
    {
        bombShootingSound.Play();
        fired = true;
        GameObject x = Instantiate(bomb, fireTransform.position, fireTransform.rotation);
        x.GetComponent<Rigidbody>().velocity =  0.25f* fireTransform.forward;
    }
    public void OnObjectSpawn()
    {
  
        shellPooler.SpawnFromPool("Shell", fireTransform.position, fireTransform.rotation, currentLaunchForce * fireTransform.forward);

    }
}

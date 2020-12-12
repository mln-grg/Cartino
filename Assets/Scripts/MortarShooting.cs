using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortarShooting : MonoBehaviour,IpooledObject
{
    public static MortarShooting instance;
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

    public Button FireButton;
    public bool canShoot = false;
    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        instance = this;
    }   
    private void Start()
    {
        FireButton.onClick.AddListener(Fire);
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
            else if (Input.GetMouseButtonDown(0))
            {
                fired = false;
                currentLaunchForce = minLaunchForce;
            }
            else if (Input.GetMouseButton(0) && !fired)
            {
                currentLaunchForce += chargeSpeed * Time.deltaTime;
            }
            else if (Input.GetMouseButtonUp(0) && !fired)
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
        PlayerPrefs.SetInt("ShotsFired", PlayerPrefs.GetInt("ShotsFired", 0) + 1);
        //EventTriggers.instance.FiredShells();
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
        PlayerPrefs.SetInt("BombsDropped", PlayerPrefs.GetInt("BombsDropped", 0) + 1);
        if (canShoot)
        {
            bombShootingSound.Play();
            fired = true;
            GameObject x = Instantiate(bomb, fireTransform.position, fireTransform.rotation);
            x.GetComponent<Rigidbody>().velocity = 0.25f * fireTransform.forward;
            canShoot = false;
            EnemiesKilled.instance.SetLocked();
        }
    }
    public void OnObjectSpawn()
    {
  
        shellPooler.SpawnFromPool("Shell", fireTransform.position, fireTransform.rotation, currentLaunchForce * fireTransform.forward);

    }
}

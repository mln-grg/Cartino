using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour,IpooledObject
{
    EnemyStates currentState;
    [SerializeField] private List<EnemyStates> states = new List<EnemyStates>();

    [SerializeField] private Vehicles enemy;
    [SerializeField] private float knockbackforce;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private bool playerInAttackRange;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject dustTrail;
        
    private Rigidbody rb;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float LaunchForce = 15f;
    
    private ParticleSystem explosionParticles;
    private PlayerHealth ph;
    private EnemyDamage enemydamage;
    private float enemyHealth;
    public float Health { get { return enemyHealth; } }
    private float enemySpeed;
    private float enemyTurnSpeed;

    private NavMeshAgent agent;
    private Transform player;
    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    
    private bool alreadyAttacked;
    private ShellPooler shellPooler;
    private bool canShoot = true;
    public bool canMove = false;

    public GameObject enemySpawnEffect;
    public GameObject postMortem;
    //private ParticleSystem postParticles;

    [SerializeField] private float explosionIntensity;
    [SerializeField] private float explosionEffectDuration;
    public AudioSource shootingSound;
    public AudioSource explodingSound;
    //public GameObject FloatingDamageText;
    //public Vector3 damagePosition;
    public GameObject collisionEffect;
    public AudioSource[] PowEffect;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        ph = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        explosionParticles = Instantiate(enemy.explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
        enemydamage = GetComponent<EnemyDamage>();
        dustTrail.gameObject.SetActive(true);
        //postParticles = Instantiate(postMortem.GetComponent<ParticleSystem>());

        canShoot = true;
        isDead = false;
        alreadyAttacked = false;
        enemyHealth = enemy.Health;
        enemySpeed = enemy.acceleration;
        enemyTurnSpeed = enemy.turnSpeed;

        agent.enabled = true;
        shellPooler = ShellPooler.GetInstance();
        StopAllCoroutines();
        

    }
    private void OnDisable()
    {
        canMove = false;
    }

    private void Update()
    {
        if(!canMove)
            StartCoroutine(spawnEffect());
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (enemyHealth <= 0)
        {

            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType().Equals(typeof(DeathState)))
                {
                    EnemyStates state = states[i];
                    ChangeState(state);
                }
            }

        }

        if (ph.IsDead)
        {
            StartCoroutine(GameOver());
        }
        if (!isDead && !ph.IsDead && enemyHealth > 0)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType().Equals(typeof(EnemyChase)) && canMove)
                {
                    EnemyStates state = states[i];
                    ChangeState(state);
                }
            }
        }
    }

    public void Chase()
    {
        agent.SetDestination(player.position);
        if (playerInAttackRange && canShoot)
            AttackPlayer();
    }
    public void Death()
    {
        isDead = true;
        EnemiesKilled.instance.IncreaseCount();
        //EventTriggers.instance.OnDeathTrigger();
        StartCoroutine(OnDeath());
    }
    private void AttackPlayer()
    {
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            StartCoroutine(Shoot());
        }
    }

    public void TakeDamage(float damage)
    {
        Pow(transform.position);
        int index = UnityEngine.Random.Range(0, PowEffect.Length);
        PowEffect[index].Play();
        enemyHealth -= damage;
        //ShowDamage(damage);
        //enemydamage.SetHealthUI();
        if (enemyHealth <= 0)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].GetType().Equals(typeof(DeathState)))
                {
                    EnemyStates state = states[i];
                    ChangeState(state);
                }
            }
        }
    }
    //void ShowDamage(float damage)
    //{
    //    GameObject x = Instantiate(FloatingDamageText, transform.position, Quaternion.identity, transform);
    //    x.transform.LookAt(Camera.main.transform);
    //    x.GetComponent<TextMesh>().text = damage.ToString();
    //}

    //public void Pow(Collision collision)
    //{
    //    collisionEffect.gameObject.transform.position = collision.contacts[0].point;
    //    collisionEffect.SetActive(true);
    //}
    public void Pow(Vector3 pos)
    {
        collisionEffect.gameObject.transform.position = pos;
        collisionEffect.SetActive(true);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        collisionEffect.SetActive(false);
    }
    public void KnockBack()
    {
        rb.AddForce(transform.forward * -1 * agent.speed * knockbackforce);
    }
    IEnumerator OnDeath()
    {
        //explodingSound.Play();
        AudioSource x = Instantiate(explodingSound, transform.position,Quaternion.identity);
        x.transform.position = transform.position;
        x.Play();
        dustTrail.gameObject.SetActive(false);
        canShoot = false;
        //enemydamage.SetHealthUI();
        isDead = true;

        agent.enabled = false;
        CameraShake.Instance.ShakeCamera(explosionIntensity, explosionEffectDuration);
        explosionParticles.transform.position = transform.position;
        //postParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        Destroy(x);


    }
    IEnumerator GameOver()
    {
        Destroy(agent);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        alreadyAttacked = true;
        shootingSound.Play();
        OnObjectSpawn();
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

    public void OnObjectSpawn()
    {
        shellPooler.SpawnFromPool("Shell", fireTransform.position, fireTransform.rotation, LaunchForce * fireTransform.forward);
    }

    public void ChangeState(EnemyStates state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = state;
        currentState.OnEnter();
    }

    
    private void OnDrawGizmos()
    {
        //  Gizmos.DrawSphere(transform.position, attackRange);
    }
    IEnumerator spawnEffect()
    {
        enemySpawnEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        enemySpawnEffect.gameObject.SetActive(false);
        canMove = true;
    }
}

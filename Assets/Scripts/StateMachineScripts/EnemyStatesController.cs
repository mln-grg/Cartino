using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyStatesController : MonoBehaviour, IpooledObject
{
    EnemyStates currentState;
    [SerializeField] List<EnemyStates> states = new List<EnemyStates>();

    [SerializeField] private Vehicles enemy;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject dustTrail;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float LaunchForce = 15f;

    
    private PlayerHealth ph;
    private EnemyDamage enemydamage;
    private ShellPooler shellPooler;
    private ParticleSystem explosionParticles;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
 

    public bool IsDead { get { return isDead; } }
    public float Health { get { return enemyHealth; } }
    private bool playerInAttackRange;
    private float enemySpeed;
    private float enemyTurnSpeed;
    private bool alreadyAttacked;
    private bool canShoot = true;
    private bool isDead = false;
    private float enemyHealth;



    private void Awake()
    {


    }
    private void Start()
    {
    }
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

    private void Update()
    {

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange && canShoot)
            AttackPlayer();

        if (ph.IsDead)
        {
            StartCoroutine(GameOver());
        }
        if (!isDead && !ph.IsDead)
        {
            agent.SetDestination(player.position);
        }

    }
    private void ChasePlayer()
    {

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
        enemyHealth -= damage;
        enemydamage.SetHealthUI();
        if (enemyHealth <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    IEnumerator OnDeath()
    {
        dustTrail.gameObject.SetActive(false);
        canShoot = false;
        enemydamage.SetHealthUI();
        isDead = true;
        agent.enabled = false;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(8f);
        gameObject.SetActive(false);

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
        OnObjectSpawn();
        yield return new WaitForSeconds(timeBetweenAttacks);

        alreadyAttacked = false;
    }

    public void OnObjectSpawn()
    {
        shellPooler.SpawnFromPool("Shell", fireTransform.position, fireTransform.rotation, LaunchForce * fireTransform.forward);
    }

    private void OnDrawGizmos()
    {
        //  Gizmos.DrawSphere(transform.position, attackRange);
    }


}
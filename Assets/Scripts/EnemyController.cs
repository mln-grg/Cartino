using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Vehicles enemy;
    [SerializeField] private float knockbackforce;
    private Rigidbody rb;
    //[SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    //[SerializeField] private bool playerInSightRange,playerInAttackRange;
    //[SerializeField] private float timeBetweenAttacks;
    //[SerializeField] private float sightRange, attackRange;
    
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
    
     //private bool alreadyAttacked;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        ph = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        explosionParticles = Instantiate(enemy.explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
        enemydamage = GetComponent<EnemyDamage>();
    }
    private void Start()
    {
        enemyHealth = enemy.Health;
        enemySpeed = enemy.acceleration;
        enemyTurnSpeed = enemy.turnSpeed;
        
    }

    private void Update()
    {
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();
        //agent.SetDestination(transform.position);
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
        //agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);
        //shooting not done yet
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        //KnockBack();
        enemydamage.SetHealthUI();
        if (enemyHealth <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }

    public void KnockBack()
    {
        rb.AddForce(transform.forward * -1 * agent.speed * knockbackforce);
    }
    IEnumerator OnDeath()
    {
        enemydamage.SetHealthUI();
        isDead = true;
        Destroy(agent);
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);        
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
    IEnumerator GameOver()
    {
        Destroy(agent);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
   
}

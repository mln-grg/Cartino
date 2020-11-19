using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Vehicles enemy;
    //[SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    //[SerializeField] private bool playerInSightRange,playerInAttackRange;
    //[SerializeField] private float timeBetweenAttacks;
    //[SerializeField] private float sightRange, attackRange;
    
    private ParticleSystem explosionParticles;
    
    private float enemyHealth;
    private float enemySpeed;
    private float enemyTurnSpeed;

    private NavMeshAgent agent;
    private Transform player;
    private bool isDead = false;
     //private bool alreadyAttacked;



    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        explosionParticles = Instantiate(enemy.explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
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
        if (!isDead)
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
        if (enemyHealth <= 0)
        {
            StartCoroutine(OnDeath());
        }
    }
    IEnumerator OnDeath()
    {
        isDead = true;          
        agent.SetDestination(transform.localPosition) ;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);        
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}

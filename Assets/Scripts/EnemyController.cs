using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour,IpooledObject
{
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

    public enum Enemy_State
    {
        Aggro,
        Death
    }
    public Enemy_State enemyState;

    private void OnEnable()
    {
        enemyState = Enemy_State.Aggro;
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

        if (enemyHealth <= 0)
        {
            enemyState = Enemy_State.Death;
        }
    
        if (ph.IsDead)
        {
            StartCoroutine(GameOver());
        }
        if (!isDead && !ph.IsDead && enemyHealth>0)
        {
            enemyState = Enemy_State.Aggro;
        }

        switch (enemyState)
        {
            case Enemy_State.Aggro:
                agent.SetDestination(player.position);
                if (playerInAttackRange && canShoot)
                    AttackPlayer();
                break;
            case Enemy_State.Death:
                isDead = true;
                StartCoroutine(OnDeath());
                break;
        }
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
            enemyState = Enemy_State.Death;
        }
    }

    public void KnockBack()
    {
        rb.AddForce(transform.forward * -1 * agent.speed * knockbackforce);
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

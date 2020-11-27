using UnityEngine;
using UnityEngine.AI;
public class EnemyChase : EnemyStates
{
    EnemyStatesController manager;

    [SerializeField] private float timeBetweenAttacks;
  
    [SerializeField] private GameObject dustTrail;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float LaunchForce = 15f;

    private PlayerHealth ph;
    private ShellPooler shellPooler;
    private ParticleSystem explosionParticles;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;

   
    private float enemySpeed;
    private float enemyTurnSpeed;
    private bool alreadyAttacked;
    private bool canShoot = true;
    private void OnEnable()
    {
        //manager = EnemyStatesController.GetInstance();
        //rb = GetComponent<Rigidbody>();
        //player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        //ph = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();
        //agent = GetComponent<NavMeshAgent>();
        //explosionParticles = Instantiate(enemy.explosionPrefab).GetComponent<ParticleSystem>();
        //explosionParticles.gameObject.SetActive(false);
        //enemydamage = GetComponent<EnemyDamage>();
        //dustTrail.gameObject.SetActive(true);

        //canShoot = true;
        //isDead = false;
        //alreadyAttacked = false;
        //enemyHealth = enemy.Health;
        //enemySpeed = enemy.acceleration;
        //enemyTurnSpeed = enemy.turnSpeed;

        agent.enabled = true;
        shellPooler = ShellPooler.GetInstance();
        StopAllCoroutines();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        agent.SetDestination(player.position);
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    private void Update()
    {
        
    }
}

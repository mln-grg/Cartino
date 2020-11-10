using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Vehicles enemy;
    private float enemyHealth;
    private float enemySpeed;
    private float enemyTurnSpeed;
    private ParticleSystem explosionParticles;

    private void Awake()
    {
        explosionParticles = Instantiate(enemy.explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
    }
    private void Start()
    {
        enemyHealth = enemy.Health;
        enemySpeed = enemy.acceleration;
        enemyTurnSpeed = enemy.turnSpeed;
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        Destroy(gameObject,3f);
    }
}

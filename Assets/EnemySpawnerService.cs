using UnityEngine;

public class EnemySpawnerService : MonoSingletonGeneric<EnemySpawnerService>
{
    [SerializeField] private PlayerHealth pc;
    [SerializeField] private float spawnTime = 3f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField]private Vehicles[] enemies;
    private float playerHealth;
    
    private void Start()
    {
        GetInstance();
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
    
    private void Spawn()
    {
        if (pc.CurrentHealth <= 0f)
        {
            return;
        }
        else
        {
            int enemiesIndex = Random.Range(0, enemies.Length);
            int spawnPointsIndex = Random.Range(0, spawnPoints.Length);
            Vector3 pos = spawnPoints[spawnPointsIndex].position;
            pos.y += 2;
            Instantiate(enemies[enemiesIndex].CarPrefab, pos, spawnPoints[spawnPointsIndex].rotation);
        }
    }
}

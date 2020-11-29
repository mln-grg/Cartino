using UnityEngine;

public class EnemySpawnerService : MonoSingletonGeneric<EnemySpawnerService>
{
    [SerializeField] private PlayerHealth pc;
    [SerializeField] private float spawnTime = 30f;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField]private Vehicles[] enemies;
    private float playerHealth;

    ShellPooler shellPooler;
    private void Start()
    {
        shellPooler = ShellPooler.GetInstance();
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
            shellPooler.SpawnFromPool("Buggy", pos, spawnPoints[spawnPointsIndex].rotation, Vector3.zero);
            //Instantiate(enemies[enemiesIndex].CarPrefab, pos, spawnPoints[spawnPointsIndex].rotation);

        }
    }
   
}

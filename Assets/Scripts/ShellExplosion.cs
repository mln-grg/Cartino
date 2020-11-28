using System.Collections;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask shouldDestroyMask;
    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private float maxLifeTime = 2f;
    [SerializeField] private float maxDamage = 100f;
    [SerializeField] private float explosionForce = 1000f;
    [SerializeField] private float explosionRadius = 5f;

    private PlayerHealth playerHealth;
    
    private void Start()
    {

    }
    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, shouldDestroyMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
            {

                explosionPrefab.Play();
                explosionAudio.Play();

                StartCoroutine(timer());
                continue;
            }
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            if (targetRigidbody.CompareTag("Player"))
            {
                playerHealth = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();
            }
            EnemyController enemyController = targetRigidbody.GetComponent<EnemyController>();
           
            float damage = CalculateDamage(targetRigidbody.position);
            
            if (!playerHealth && enemyController)
            {
                
                enemyController.TakeDamage(damage);
            }
            else if (!enemyController && playerHealth)
            {
          
                playerHealth.TakeDamage(damage);
            }
            

            explosionPrefab.Play();
            explosionAudio.Play();
 
            StartCoroutine(timer());
        }
    }
    private float CalculateDamage(Vector3 targetPosition)
    {
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
        float damage = relativeDistance * maxDamage;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(explosionPrefab.main.duration);
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
          //Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
//player 1000000
//enemy 1000
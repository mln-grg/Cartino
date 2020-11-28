using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] private float collisionWithEnemyDamage;
    [SerializeField] private float collisionWithNonDestructibleDamage;
    [SerializeField] private float collisionWithDestructibleDamage;
    [SerializeField] private float nextCollisionDamageOtherDelay = 3f;
    [SerializeField] private float nextCollisionDamageDelay = 1f;
    [SerializeField] private Vector3 halfExtents;

    public GameObject turret;
    public Transform turrrrrret;

    [SerializeField]private LayerMask damageLayer;

    //private Rigidbody rigidBody;
    private PlayerHealth playerHealth;

    private bool canTakeDamage = true;
    private void Awake()
    {
        //rigidBody = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Entered Collision fn");
    //    Debug.Log(collision.gameObject.name);
    //    if (canTakeDamage) 
    //    {
    //        //Vector3 velocity = rigidBody.velocity;
    //        if (playerHealth.IsDead == false)
    //        {
    //            if (collision.gameObject.GetComponent<EnemyController>()!=null)
    //            {
    //                Debug.Log("Entered enemy Collision fn");
    //                StartCoroutine(damage(collisionWithEnemyDamage , nextCollisionDamageDelay));
    //            }

    //            if (collision.gameObject.CompareTag("Destructible"))
    //            {
    //                Debug.Log("Entered Destructible Collision fn");
    //                StartCoroutine(damage(collisionWithNonDestructibleDamage , nextCollisionDamageDelay));
    //            }

    //            if (collision.gameObject.CompareTag("NonDestructible"))
    //            {
    //                Debug.Log("Entered nonDestructible Collision fn");
    //                StartCoroutine(damage(collisionWithNonDestructibleDamage , nextCollisionDamageOtherDelay));
    //            }
    //        }                                  
    //    }
    //}

    private void FixedUpdate()
    {
        if (!playerHealth.IsDead)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, Quaternion.identity, damageLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("Enemy") && canTakeDamage)
                {
          
                    StartCoroutine(damage(collisionWithEnemyDamage, nextCollisionDamageDelay));
                }

                if (colliders[i].gameObject.CompareTag("Destructible") && canTakeDamage)
                {
          
                    StartCoroutine(damage(collisionWithNonDestructibleDamage, nextCollisionDamageDelay));
                }

                if (colliders[i].gameObject.CompareTag("NonDestructible") && canTakeDamage)
                {
      
                    StartCoroutine(damage(collisionWithNonDestructibleDamage, nextCollisionDamageOtherDelay));
                }
            }
        }
    }

    IEnumerator damage(float damage, float delay)
    {

        playerHealth.TakeDamage(damage);
        canTakeDamage = false;
        yield return new WaitForSeconds(delay);
        canTakeDamage = true;

        

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, halfExtents * 2f);
    }
}


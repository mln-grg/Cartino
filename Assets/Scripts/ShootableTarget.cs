using UnityEngine;

public class ShootableTarget : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}

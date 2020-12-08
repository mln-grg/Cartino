using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float collisionWithPlayerDamage;
    [SerializeField] private float collisionWithNonDestructibleDamage;
    [SerializeField] private float collisionWithDestructibleDamage;
    [SerializeField] private float nextCollisionDamageOtherDelay = 3f;
    [SerializeField] private float nextCollisionDamageDelay = 1f;


    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;

    private EnemyController enemyController;

    private bool canTakeDamage = true;
    private float currentHealth;
    private float health;
    public GameObject collisionEffect;

    private void OnEnable()
    {
       
        currentHealth = enemyController.Health;
        health = currentHealth;
        //SetHealthUI();
        canTakeDamage = true;

        StopAllCoroutines();
    }
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (enemyController.IsDead == true)
        {
            StopAllCoroutines();
        }
        //SetHealthUI();
        currentHealth = enemyController.Health;
    }

    //public void SetHealthUI()
    //{
    //    slider.value = currentHealth;
    //    fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, (currentHealth / health));
    //}
    private void OnCollisionEnter(Collision collision)
    {

    if (enemyController.IsDead == false)
    {
            if (collision.gameObject.GetComponent<EnemyController>()!=null && canTakeDamage)
            {
                StartCoroutine(damage(300f, nextCollisionDamageDelay));
            }
            if (collision.gameObject.CompareTag("Player") && canTakeDamage)
            {
                Pow(collision);
                
                StartCoroutine(damage(collisionWithPlayerDamage, nextCollisionDamageDelay));
            }
            if (collision.gameObject.CompareTag("Destructible") && canTakeDamage)
            {

                StartCoroutine(damage(collisionWithNonDestructibleDamage, nextCollisionDamageDelay));
            }

            if (collision.gameObject.CompareTag("NonDestructible") &&canTakeDamage )
            {
                StartCoroutine(damage(collisionWithNonDestructibleDamage, nextCollisionDamageOtherDelay));
            }
    }
    }
    IEnumerator damage(float damage, float delay)
    {
        //SetHealthUI();
        enemyController.TakeDamage(damage);
        canTakeDamage = false;
        yield return new WaitForSeconds(delay);
        collisionEffect.SetActive(false);
        canTakeDamage = true;
    }

    
    IEnumerator Disablewait()
    {
        yield return new WaitForSeconds(1f);
        collisionEffect.SetActive(false);
    }

    public void Pow(Collision collision)
    {
        collisionEffect.gameObject.transform.position = collision.contacts[0].point;
        collisionEffect.SetActive(true);
    }
    public void Pow(Vector3 pos)
    {
        collisionEffect.gameObject.transform.position = pos;
        collisionEffect.SetActive(true);
    }
}

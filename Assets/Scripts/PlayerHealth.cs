using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;
    [SerializeField] GameObject explosionPrefab;

    private ParticleSystem explosionParticles;
    private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    private bool isDead;

    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = health;
        isDead = false;

        SetHealthUI();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        SetHealthUI();
        if(currentHealth<=0f && !isDead)
        {
            //OnDeath();
            StartCoroutine(OnDeath());
        }
    }
    private void Update()
    {
        SetHealthUI();
    }
    private void SetHealthUI()
    {
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, (currentHealth/health));
    }
    private IEnumerator OnDeath()
    {
        isDead = true;
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        yield return  new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}

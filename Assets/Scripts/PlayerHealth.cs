using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject postexplosionPrefab;
    [SerializeField] GameObject dustTrail;

    private ParticleSystem explosionParticles;
    private ParticleSystem postExplosionParticles;
    [SerializeField]private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    private bool isDead;

    private void Awake()
    {
        explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        explosionParticles.gameObject.SetActive(false);
        postExplosionParticles = Instantiate(postexplosionPrefab).GetComponent<ParticleSystem>();
        postExplosionParticles.gameObject.SetActive(false);
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
    IEnumerator OnDeath()
    {
        isDead = true;
        gameObject.GetComponent<PlayerInputs>().enabled = false;
        dustTrail.SetActive(false);
        yield return new WaitForSeconds(1f);
        
        explosionParticles.transform.position = transform.position;
        explosionParticles.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        
        //postExplosionParticles.transform.position = transform.position;
        Vector3 offset = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
        postExplosionParticles.transform.position = offset;
        postExplosionParticles.gameObject.SetActive(true);       
       
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }
}

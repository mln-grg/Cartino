﻿using UnityEngine;
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
    [SerializeField] private Canvas playerCanvas;

    private ParticleSystem explosionParticles;
    private ParticleSystem postExplosionParticles;
    [SerializeField]private float currentHealth;
    private PlayerController pc;
    public float CurrentHealth { get { return currentHealth; } }
    private bool isDead;
    public bool IsDead { get { return isDead; } }

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
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
            //pc.KnockBack();
        SetHealthUI();
        if(currentHealth<=0f && !isDead)
        {
            StartCoroutine(OnDeath());
        }
    }
    private void Update()
    {
        if(IsDead == true)
        {
            pc.freezePlayer();    
        }
        SetHealthUI();
    }
    private void SetHealthUI()
    {
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, (currentHealth/health));
    }
    IEnumerator OnDeath()
    {
        gameObject.GetComponent<PlayerInputs>().enabled = false;
       
        isDead = true;
        dustTrail.SetActive(false);
        Destroy(playerCanvas);
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

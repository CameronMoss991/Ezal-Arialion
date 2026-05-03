using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public int scoreValue = 10;
    private float currentHealth;
    private Renderer enemyRenderer;

    [Header("Drops")]
    public GameObject shieldPowerUpPrefab; 
    [Range(0, 1)] public float dropChance = 0.5f;

    void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }
        UpdateVisuals();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (enemyRenderer != null)
            UpdateVisuals();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void UpdateVisuals()
    {
        if (enemyRenderer == null) return;
        // Calculate health as a percentage (0.0 to 1.0)
        float healthPercent = currentHealth / maxHealth;

        // Lerp: At 1.0, it's Green. At 0.0, it's Red.
        // As it drops, it will naturally turn Yellow/Orange.
        enemyRenderer.material.color = Color.Lerp(Color.red, Color.green, healthPercent);
    }

    void Die()
    {
        // 1. Tell the GameManager to add score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        // 2. Chance to drop a shield power-up
        if (Random.value <= dropChance && shieldPowerUpPrefab != null)
        {
            Instantiate(shieldPowerUpPrefab, transform.position, Quaternion.identity);
        }

        // 3. Remove enemy from scene
        Destroy(gameObject);
    }
}
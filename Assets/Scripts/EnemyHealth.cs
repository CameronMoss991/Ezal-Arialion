using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public int scoreValue = 10;
    private float currentHealth;

    [Header("Drops")]
    public GameObject shieldPowerUpPrefab; 
    [Range(0, 1)] public float dropChance = 0.5f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
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
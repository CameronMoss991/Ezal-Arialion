using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHealth = 100f;
    public int baseScore = 10;
    
    [Header("Dynamic Stats (Do not edit)")]
    public float maxHealth;
    public int scoreValue;
    private float currentHealth;
    private Renderer enemyRenderer;

    [Header("Drops")]
    public GameObject shieldPowerUpPrefab; 
    [Range(0, 1)] public float dropChance = 0.5f;

    void Start()
    {
        // 1. Check the time
        float difficulty = GameManager.Instance.GetDifficultyProgress();
        
        // 2. Randomize Size (Later game = chance for much bigger enemies)
        float sizeMultiplier = Random.Range(1.0f, 1.3f + (difficulty * 0.7f));
        
        // Scale the physical model (X, Y, Z)
        transform.localScale = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier);

        // 3. Set Health and Score based on Size AND Time
        maxHealth = (baseHealth * sizeMultiplier) + (50f * difficulty);
        scoreValue = Mathf.RoundToInt(baseScore * sizeMultiplier) + Mathf.RoundToInt(15 * difficulty);
        
        currentHealth = maxHealth;

        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer == null) enemyRenderer = GetComponentInChildren<Renderer>();
            
        UpdateVisuals();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (enemyRenderer != null) UpdateVisuals();
        if (currentHealth <= 0) Die();
    }

    void UpdateVisuals()
    {
        if (enemyRenderer == null) return;
        float healthPercent = currentHealth / maxHealth;
        enemyRenderer.material.color = Color.Lerp(Color.red, Color.green, healthPercent);
    }

    void Die()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddScore(scoreValue);
        
        if (Random.value <= dropChance && shieldPowerUpPrefab != null)
        {
            Instantiate(shieldPowerUpPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
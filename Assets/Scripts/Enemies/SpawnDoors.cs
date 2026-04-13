using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoors : MonoBehaviour
{
    public GameObject enemyPrefab;    // Drag your Enemy 1 Prefab here
    public float spawnRate = 3.0f;    // Seconds between spawns
    public int maxEnemies = 10;       // Prevent lag by capping enemy count

    private float nextSpawnTime;

    void Update()
    {
        // Don't spawn if the game is over
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        // Check if it's time to spawn and if we are under the limit
        if (Time.time >= nextSpawnTime)
        {
            // Only spawn if we haven't hit the cap
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
            {
                SpawnEnemy();
            }
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}

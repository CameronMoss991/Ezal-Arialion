using UnityEngine;

public class SpawnDoors : MonoBehaviour
{
    public GameObject enemyPrefab;    
    private float nextSpawnTime;

    void Start()
    {
        // Still use the random offset so they don't sync up perfectly
        nextSpawnTime = Time.time + Random.Range(0f, GameManager.Instance.startSpawnRate);
    }

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.isGameOver) return;

        // 1. Get the difficulty from the clock
        float difficulty = GameManager.Instance.GetDifficultyProgress();
        // 2. READ values from the Global GameManager instead of local variables
        float currentSpawnRate = Mathf.Lerp(GameManager.Instance.startSpawnRate, GameManager.Instance.endSpawnRate, difficulty);
        int currentMaxEnemies = GameManager.Instance.startMaxEnemies + Mathf.RoundToInt((GameManager.Instance.endMaxEnemies - GameManager.Instance.startMaxEnemies) * difficulty);

        // 3. Spawn Logic
        if (Time.time >= nextSpawnTime)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < currentMaxEnemies)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
            nextSpawnTime = Time.time + currentSpawnRate + Random.Range(-0.1f, 0.1f);
        }
    }
}
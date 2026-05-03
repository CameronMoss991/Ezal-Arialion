using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText; // Add a new TMP for the countdown
    public GameObject gameOverPanel;

    [Header("Game Settings")]
    public int score = 0;
    public float levelTimeLimit = 120f; // 2 minutes to survive
    private float currentTime;
    public bool isGameOver = false;

    [Header("Global Spawn Balancing")]
    public float startSpawnRate = 4.0f;    
    public float endSpawnRate = 0.5f;   
    public int startMaxEnemies = 15;       
    public int endMaxEnemies = 50; 

    // We can even add the "Speed Scaling" here so all enemies move at the same speed
    public float startEnemySpeed = 8f;
    public float endEnemySpeed = 18f;

    void Awake() 
    { 
        if (Instance == null) Instance = this; 
        
        // Read the time chosen from the Main Menu. If none was chosen, default to 300s (5 mins)
        levelTimeLimit = PlayerPrefs.GetFloat("LevelDuration", 300f);
        currentTime = levelTimeLimit;
    }

    // Difficulty HELPER FUNCTION
    // All enemies will call this to see how hard it will be
    public float GetDifficultyProgress()
    {
        // Returns 0.0 at the start of the timer, and exactly 1.0 when the timer ends!
        return Mathf.Clamp01(Time.timeSinceLevelLoad / levelTimeLimit);
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateTimer();
        }
    }

    // --- Score Logic ---
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public bool SpendScore(int amount)
    {
        if (score >= amount)
        {
            score -= amount;
            UpdateScoreUI();
            return true; // Purchase successful
        }
        return false; // Not enough points!
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Points: " + score;
    }

    // --- Timer Logic ---
    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame(true); // You survived!
        }

        if (timerText != null)
        {
            // Formats time as 0:00
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    // --- Ending Logic ---
    public void EndGame(bool victory = false)
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        
        // Change text to Victory if they survived
        if (victory) 
            gameOverPanel.GetComponentInChildren<TextMeshProUGUI>().text = "LEVEL COMPLETE!";

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
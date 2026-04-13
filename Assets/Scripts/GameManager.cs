using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText; // Drag ScoreText here
    public GameObject gameOverPanel;  // Drag GameOverPanel here

    public bool isGameOver = false;

    void Awake() { if (Instance == null) Instance = this; }

    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void EndGame()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Freezes the game world
        Cursor.lockState = CursorLockMode.None; // Frees the mouse
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Unfreeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
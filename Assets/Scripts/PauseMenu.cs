using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject instructionsPanel;
    public Slider musicSlider;
    public AudioSource backgroundMusic; // Drag your Music GameObject here

    void Start()
    {
        // Load the saved volume preference (default to 0.75)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        musicSlider.value = savedVolume;
        if (backgroundMusic != null) backgroundMusic.volume = savedVolume;
        
        musicSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // If instructions are open, "Go Back" to the Pause Menu
                if (instructionsPanel.activeSelf)
                {
                    CloseInstructions();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void OpenInstructions()
    {
        instructionsPanel.SetActive(true);
        pauseMenuUI.SetActive(false); // Hide the main pause buttons
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        pauseMenuUI.SetActive(true); // Bring back the main pause buttons
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        instructionsPanel.SetActive(false); // Close instructions if they were open
        Time.timeScale = 1f;
        isPaused = false;

        // Re-lock the cursor so the player can aim again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
        isPaused = true;

        // Free the cursor to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            // This ensures the volume is never "null" and updates correctly
            backgroundMusic.volume = volume;
            
            // Mute the audio source entirely if volume is near 0 to save processing
            backgroundMusic.mute = (volume <= 0.01f);
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void ToggleInstructions()
    {
        instructionsPanel.SetActive(!instructionsPanel.activeSelf);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Unfreezes time before switching scenes
        SceneManager.LoadScene("MainMenu"); // Make sure this matches your scene name!
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
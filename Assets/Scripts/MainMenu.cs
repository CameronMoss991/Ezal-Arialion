using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "SampleScene"; // EXACT name of your game scene!

    // Wire your 5 buttons to these methods
    public void Select5Minutes() => StartGame(300f);
    public void Select10Minutes() => StartGame(600f);
    public void Select15Minutes() => StartGame(900f);
    public void Select30Minutes() => StartGame(1800f);
    public void Select1Hour() => StartGame(3600f);

    private void StartGame(float durationInSeconds)
    {
        // Save the chosen time to Unity's memory
        PlayerPrefs.SetFloat("LevelDuration", durationInSeconds);
        PlayerPrefs.Save();

        // Load the Colosseum scene
        SceneManager.LoadScene(gameSceneName);
    }
}
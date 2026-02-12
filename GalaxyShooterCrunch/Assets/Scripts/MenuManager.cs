using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    
    void Start()
    {
        // Add listeners to buttons
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        
        // Make sure time is running normally (in case coming from game over)
        Time.timeScale = 1f;
    }
    
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("SampleScene"); // Your game scene name
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        
        // For testing in Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    // Optional: Also allow Enter key to start
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
}
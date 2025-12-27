using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Settings")]
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    
    void Start()
    {
        ShowMainMenu();
    }
    
    void Update()
    {
        if (mainMenuPanel.activeSelf && Input.anyKeyDown)
        {
            StartGame();
        }
        
        if (gameOverPanel.activeSelf && Input.anyKeyDown)
        {
            RestartGame();
        }
    }
    
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    
    public void ShowGameOver()
    {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        
        GameController gameController = FindFirstObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.Run();
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject pauseMenuUI;

    private bool isPaused = false;
    public static bool GameIsPaused = false;
    public static float lastUnpauseTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume time
        isPaused = false;

        AudioListener.pause = false; // resume sound with game

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameIsPaused = false;

        lastUnpauseTime = Time.unscaledTime; // prevents fire when clicking in the menu
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f; // Freeze game
        isPaused = true;
        GameIsPaused = true;

        AudioListener.pause = true; // stop sound when game is paused

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        GameIsPaused = false;
        lastUnpauseTime = Time.unscaledTime; // prevents fire when clicking in the menu
        GunScriptBase.isReloading = false;

        AudioListener.pause = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

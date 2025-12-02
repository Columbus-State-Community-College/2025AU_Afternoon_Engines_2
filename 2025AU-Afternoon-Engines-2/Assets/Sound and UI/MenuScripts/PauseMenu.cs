using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject firstButton;
    public GameObject pauseMenuUI;
    
    private bool isPaused = false;
    public static bool GameIsPaused = false;
    public static float lastUnpauseTime;

    private InputSystems controls;
    private bool pausePressed;

    private void Awake()
    {
        controls = new InputSystems();

        // Assign pause button (e.g., Start or Options on controller)
        controls.Player.Pause.performed += ctx => pausePressed = true;
        controls.Player.Pause.canceled += ctx => pausePressed = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (pausePressed)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();

            pausePressed = false;
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

        EventSystem.current.SetSelectedGameObject(null); // clear
        EventSystem.current.SetSelectedGameObject(firstButton);

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

        PerkChecker.hasDoubleHealth = false;
        PerkChecker.hasSpeedReload = false;
        PerkChecker.hasFasterMovement = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

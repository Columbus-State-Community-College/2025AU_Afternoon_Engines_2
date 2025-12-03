using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;

    private bool isPaused = false;
    public static bool GameIsPaused = false;
    public static float lastUnpauseTime;

    private bool pausePressed;

    private InputSystems controls;

    private void Awake()
    {
        controls = new InputSystems();

        // assign pause button
        controls.Player.Pause.performed += ctx => pausePressed = true;
        controls.Player.Pause.canceled += ctx => pausePressed = false;
    }

    private void OnEnable()
    {
        if (controls != null)
            controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    void Update()
    {
        if (pausePressed)
        {
            // only toggle if controls menu isn't open
            if (controlsMenuUI != null && controlsMenuUI.activeSelf)
            {
                pausePressed = false;
                return;
            }
            else
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
            pausePressed = false;
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);

        Time.timeScale = 1f;
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
        controlsMenuUI.SetActive(false); // hide controls if open

        Time.timeScale = 0f;
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

        PerkChecker.hasDoubleHealth = false;
        PerkChecker.hasSpeedReload = false;
        PerkChecker.hasFasterMovement = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenControlsPanel()
    {
        if (controlsMenuUI != null)
            controlsMenuUI.SetActive(true);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // hide pause menu

        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseControlsPanel()
    {
        if (controlsMenuUI != null)
            controlsMenuUI.SetActive(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true); // return to pause menu

        Time.timeScale = 0f; // keep game paused
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

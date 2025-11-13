using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject controlsPanel;

    public void StartGame()
    {
        PauseMenu.GameIsPaused = false;
        PauseMenu.lastUnpauseTime = -1f;
        GunScriptBase.isReloading = false;

        AudioListener.pause = false; // lets audio start again if the player goes to back main menu and clicks start

        SceneManager.LoadScene("TestingArea");
    }

    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

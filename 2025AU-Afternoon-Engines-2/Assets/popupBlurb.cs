using UnityEngine;

public class popupBlurb : MonoBehaviour
{
    public GameObject instructionsUI;
    public float duration = 30f; 

    private float timer;
    private bool popupTimeActive = false;

    void Start()
    {
        // show when game is started
        if (instructionsUI != null)
        {
            instructionsUI.SetActive(true);
            timer = duration;
            popupTimeActive = true;
        }
    }

    void Update()
    {
        // if paused hide the popup
        if (PauseMenu.GameIsPaused)
        {
            if (instructionsUI.activeSelf)
                instructionsUI.SetActive(false);

            return;
        }

        // if not paused and time is still remaining, keep showing the popup
        if (popupTimeActive)
        {
            timer -= Time.unscaledDeltaTime;

            if (timer > 0f)
            {
                // show popup within 30 second window
                if (!instructionsUI.activeSelf)
                    instructionsUI.SetActive(true);
            }
            else
            {
                // when time is up hide the popup until game is restarted
                instructionsUI.SetActive(false);
                popupTimeActive = false;
            }
        }
    }
}
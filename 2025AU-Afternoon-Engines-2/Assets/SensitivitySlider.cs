using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public PlayerCamera playerCamera; // assign in inspector
    public Slider sliderX;
    public Slider sliderY;
    public bool isMouse = true; // true = mouse, false = controller

    private void Start()
    {
        // slider values
        if (isMouse)
        {
            sliderX.value = playerCamera.mouseSensX;
            sliderY.value = playerCamera.mouseSensY;
        }
        else
        {
            sliderX.value = playerCamera.controllerSensX;
            sliderY.value = playerCamera.controllerSensY;
        }

        sliderX.onValueChanged.AddListener(UpdateX);
        sliderY.onValueChanged.AddListener(UpdateY);
    }

    private void UpdateX(float value)
    {
        if (isMouse) playerCamera.mouseSensX = value;
        else playerCamera.controllerSensX = value;
    }

    private void UpdateY(float value)
    {
        if (isMouse) playerCamera.mouseSensY = value;
        else playerCamera.controllerSensY = value;
    }

    private void UpdateSensitivity(float value)
    {
        playerCamera.mouseSensX = value; // update camera sensitivity in real-time
        playerCamera.mouseSensY = value;
    }
}

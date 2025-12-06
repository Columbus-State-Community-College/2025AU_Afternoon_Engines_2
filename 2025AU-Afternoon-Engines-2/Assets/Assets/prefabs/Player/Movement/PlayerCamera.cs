using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensX = 0.15f;
    public float mouseSensY = 0.15f;
    public float controllerSensX = 400f;
    public float controllerSensY = 400f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private InputSystems controls;
    private Vector2 lookInput;
    private bool usingMouse;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        controls = new InputSystems();

        controls.Player.Look.performed += ctx =>
        {
            lookInput = ctx.ReadValue<Vector2>();

            // detect which device input is coming from
            usingMouse = ctx.control.device is Mouse;
        };
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        Vector2 currentLook = lookInput;

        if (Mouse.current != null && Mouse.current.wasUpdatedThisFrame)
        {
            float mouseX = currentLook.x * mouseSensX;
            float mouseY = currentLook.y * mouseSensY;

            yRotation += mouseX;
            xRotation -= mouseY;
        }
        else
        {
            float controllerX = currentLook.x * controllerSensX * Time.deltaTime;
            float controllerY = currentLook.y * controllerSensY * Time.deltaTime;

            yRotation += controllerX;
            xRotation -= controllerY;
        }

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

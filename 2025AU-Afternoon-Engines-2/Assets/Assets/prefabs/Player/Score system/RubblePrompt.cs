using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RubblePrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;

    public float inteRange = 3f;
    private bool isLookingAway = true;

    private InputSystems controls;
    private bool interactPressed;

    void Start()
    {
        setText();
    }

    private void Awake()
    {
        controls = new InputSystems();

        // interact button
        controls.Player.Interact.performed += ctx => interactPressed = true;
        controls.Player.Interact.canceled += ctx => interactPressed = false;
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
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        bool isVisibleAndFacing = IsObjectVisibleAndFacing(cam, this.gameObject);

        GameObject parent = transform.parent.gameObject;

        if (dist <= inteRange && isVisibleAndFacing)
        {
            promptTex.gameObject.SetActive(true);
            isLookingAway = true;

            if (interactPressed)
            {
                // Use ScoreManager for UI update modified by thomas
                if (ScoreManager.instance.SpendPoints(1500))
                {
                    promptTex.gameObject.SetActive(false);
                    parent.SetActive(false);
                }
                interactPressed = false;
            }
        }
        else if (isLookingAway)
        {
            promptTex.gameObject.SetActive(false);
            isLookingAway = false;
        }
    }

    bool IsObjectVisibleAndFacing(Camera cam, GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (!GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Renderer>().bounds))
            return false;

        Vector3 directionToObject = (obj.transform.position - cam.transform.position).normalized;
        float angle = Vector3.Dot(cam.transform.forward, directionToObject);

        return angle > 0.7f;
    }

    void setText()
    {
        promptTex.text = "Press E to remove rubble (1500)";
    }
}

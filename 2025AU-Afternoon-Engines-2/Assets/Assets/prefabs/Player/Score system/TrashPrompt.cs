using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TrashPrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;

    public float inteRange = 3f;
    public float cooldown = 0f;
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
        setText();
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        bool isVisibleAndFacing = IsObjectVisibleAndFacing(cam, this.gameObject);

        if (dist <= inteRange && isVisibleAndFacing)
        {
            promptTex.gameObject.SetActive(true);
            isLookingAway = true;

            if (interactPressed)
            {
                // Use ScoreManager for UI update modified by thomas
                if (cooldown <= 0f)
                {
                    cooldown = 25f;
                    GunScriptBase.reserve += 9;
                    AssaultRifle.reserve += 15;
                    BoltActionRifle.reserve += 6;
                }
                
            }
        }
        else if (isLookingAway)
        {
            promptTex.gameObject.SetActive(false);
            isLookingAway = false;
        }
        if (cooldown > 0) {
            cooldown -= Time.deltaTime;
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
        if (cooldown <= 0) {
            promptTex.text = "Press E to search for Ammo";
        }
        else {
            promptTex.text = "On Cooldown..";
        }
        
    }
}

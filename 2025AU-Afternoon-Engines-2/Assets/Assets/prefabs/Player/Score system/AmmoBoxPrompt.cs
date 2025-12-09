using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AmmoBoxPrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;
    public TextMeshProUGUI ammoTex;

    private float inteRange = 3f;

    private InputSystems controls;
    private bool interactPressed;



    void Start()
    {
        setText();
    }

    private void Awake()
    {
        controls = new InputSystems();

        // Set interact button
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

        if (dist <= inteRange && isVisibleAndFacing)
        {
            promptTex.gameObject.SetActive(true);

            if (interactPressed)
            {                       // modified by thomas
                if (ScoreManager.instance.SpendPoints(1000))
                {
                    GunScriptBase.reserve = GunScriptBase.maxReserve;
                    BoltActionRifle.reserve = GunScriptBase.maxReserve;
                    AssaultRifle.reserve = GunScriptBase.maxReserve;

                    if (GunHandler.hasBolt) {
                        ammoTex.text = 3 + "/" + 35;
                    }
                    if (GunHandler.hasAR) {
                        ammoTex.text = 20 + "/" + 200;
                    }
                    if (GunHandler.hasPistol) {
                        ammoTex.text = 7 + "/" + 150;
                    }
                }
                interactPressed = false;
            }
        }
        else
        {
            promptTex.gameObject.SetActive(false);
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
        promptTex.text = "Press E to Buy Ammo (1000)";
    }
}

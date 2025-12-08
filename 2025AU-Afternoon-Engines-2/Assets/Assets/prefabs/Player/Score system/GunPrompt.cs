using UnityEngine;
using TMPro;

public class GunPrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;
    public GameObject otherGun;

    public int cost = 0;
    public int gunType = 0;
    public string gunName = "gun";

    private float inteRange = 2f;
    private bool isLookingAway = true;

    private InputSystems controls;
    private bool interactPressed;
    

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

            setText();

            if (interactPressed)
            {
                // Use ScoreManager for UI update modified by thomas
                if (ScoreManager.instance.SpendPoints(cost))
                {
                    promptTex.gameObject.SetActive(false);
                    parent.SetActive(false);
                    otherGun.SetActive(true);
                    setGun();
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
        promptTex.text = "Press E to buy " + gunName + " (" + cost.ToString() + ")";
    }
    void setGun() {
        if (gunType == 1) {
            GunHandler.hasBolt = false;
            GunHandler.hasPistol = false;
            GunHandler.hasAR = true;
            GunScriptBase.reserve = 200;
            GunScriptBase.maxReserve = 200;
        }
        if (gunType == 2) {
            GunHandler.hasBolt = true;
            GunHandler.hasPistol = false;
            GunHandler.hasAR = false;
            GunScriptBase.reserve = 35;
            GunScriptBase.maxReserve = 35;
        }
    }
}

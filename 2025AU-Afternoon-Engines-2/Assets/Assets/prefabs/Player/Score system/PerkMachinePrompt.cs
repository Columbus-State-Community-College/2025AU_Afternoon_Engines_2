using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PerkMachinePrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;

    private float inteRange = 5f;

    public static int bottlevalue = 1; 
    public static int perkChosen = 0;
    // 0: Nothing, 1: DoubleHealth, 2: FasterMovement, 3: SpeedReload

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

        if (dist <= inteRange && isVisibleAndFacing)
        {
            promptTex.gameObject.SetActive(true);

            if (interactPressed)
            {
                // Require 3000 pts to use
                if (ScoreManager.instance.SpendPoints(3000))
                {
                    bool hasHealthPerk = PerkChecker.hasDoubleHealth;
                    bool hasSpeedPerk = PerkChecker.hasFasterMovement;
                    bool hasReloadPerk = PerkChecker.hasSpeedReload;

                    
                    if (!hasHealthPerk || !hasSpeedPerk || !hasReloadPerk)
                    {
                        gamble();
                    }
                    interactPressed = false;
                }
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

    void gamble()
    {
        perkChosen = Random.Range(1, 4);

        bool hasHealthPerk = PerkChecker.hasDoubleHealth;
        bool hasSpeedPerk = PerkChecker.hasFasterMovement;
        bool hasReloadPerk = PerkChecker.hasSpeedReload;

        bool rerolling = true;

        while (rerolling)
        {
            perkChosen = Random.Range(1, 4);

            if (hasHealthPerk && perkChosen == 1) { }
            else if (hasSpeedPerk && perkChosen == 2) { }
            else if (hasReloadPerk && perkChosen == 3) { }
            else
            {
                rerolling = false;
            }
        }

        DrinkBehavior.recievedPerk = perkChosen;
    }

    void setText()
    {
        promptTex.text = "Press E to Buy Perk (3000)";
    }
}

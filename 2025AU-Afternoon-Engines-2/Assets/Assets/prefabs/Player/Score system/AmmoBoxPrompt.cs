using UnityEngine;
using TMPro;

public class AmmoBoxPrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;

    private float inteRange = 3f;

    void Start()
    {
        setText();
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        bool isVisibleAndFacing = IsObjectVisibleAndFacing(cam, this.gameObject);

        if (dist <= inteRange && isVisibleAndFacing)
        {
            promptTex.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {                       // modified by thomas
                if (ScoreManager.instance.SpendPoints(1250))
                {
                    GunScriptBase.reserve = GunScriptBase.maxReserve;
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

    void setText()
    {
        promptTex.text = "Press E to Buy Ammo (1250)";
    }
}

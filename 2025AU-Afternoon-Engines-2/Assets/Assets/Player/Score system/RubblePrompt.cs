using UnityEngine;
using TMPro;

public class RubblePrompt : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI promptTex;
    private float inteRange = 3f;
    private bool isLookingAway = true;
    
    void Start()
    {
        setText();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        bool isVisibleAndFacing = IsObjectVisibleAndFacing(cam, this.gameObject);
        GameObject parent = transform.parent.gameObject;
        if (dist <= inteRange && isVisibleAndFacing) {
            promptTex.gameObject.SetActive(true); 
            isLookingAway = true;
            if (Input.GetKeyDown(KeyCode.E) && ScoreManager.currentScore >= 300)
            {
                ScoreManager.currentScore -= 300;
                promptTex.gameObject.SetActive(false);
                parent.SetActive(false); // disables the rubble
            }
        }
        else if (isLookingAway) {
            promptTex.gameObject.SetActive(false);
            isLookingAway = false;
        }
        
    }
    bool IsObjectVisibleAndFacing(Camera cam, GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (!GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Renderer>().bounds)) {
            return false;
        }
        Vector3 directionToObject = (obj.transform.position - cam.transform.position).normalized;
        float angle = Vector3.Dot(cam.transform.forward, directionToObject);

        return angle > 0.7f;
    }
    void setText() {
        promptTex.text = "Press E to Remove Rubble for 300";
    }
}

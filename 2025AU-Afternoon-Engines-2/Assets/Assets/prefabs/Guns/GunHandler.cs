using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public static bool hasPistol = true;
    public static bool hasAR = false;
    public static bool hasBolt = false;
    public GameObject Pistol;
    public GameObject AR;
    public GameObject BoltActionRifle;
    void Start()
    {
        
    }

    void Update()
    {
        if (!DrinkBehavior.isDrinking) {
            if (hasPistol) {
                Pistol.gameObject.SetActive(true);
            }
            else {
                Pistol.gameObject.SetActive(false);
            }
            if (hasAR) {
                AR.gameObject.SetActive(true);
            }
            else {
                AR.gameObject.SetActive(false);
            }
            if (hasBolt) {
                BoltActionRifle.gameObject.SetActive(true);
            }
            else {
                BoltActionRifle.gameObject.SetActive(false);
            }
        }
    }
}

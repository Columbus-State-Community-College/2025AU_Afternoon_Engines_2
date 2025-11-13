using UnityEngine;
using System.Collections;
using TMPro;

public class DrinkBehavior : MonoBehaviour
{
    public static int recievedPerk = 0;
    public GameObject pMachine;
    public GameObject gun;
    public TextMeshProUGUI promptTex;
    private float drinkDuration = 3f;
    private bool isDrinking = false;

    void Update()
    {
        if (recievedPerk != 0 && !isDrinking) 
        {
            StartCoroutine(DrinkRoutine());
        }
    }

    IEnumerator DrinkRoutine()
    {
        isDrinking = true;
        gun.SetActive(false);
        pMachine.SetActive(false);
        promptTex.enabled = false;
        yield return new WaitForSeconds(drinkDuration); 
        
        
        if (recievedPerk == 1) {
            PerkChecker.hasDoubleHealth = true;
        }
        if (recievedPerk == 2) {
            PerkChecker.hasFasterMovement = true;
        }
        if (recievedPerk == 3) {
            PerkChecker.hasSpeedReload = true;
        }
        gun.SetActive(true);
        pMachine.SetActive(true);
        promptTex.enabled = true;
        GunScriptBase.isReloading = false;
        recievedPerk = 0; 
        
        isDrinking = false;
    }
}
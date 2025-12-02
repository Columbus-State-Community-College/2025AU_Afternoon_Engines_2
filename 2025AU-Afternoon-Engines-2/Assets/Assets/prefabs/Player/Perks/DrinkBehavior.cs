using UnityEngine;
using System.Collections;
using TMPro;

public class DrinkBehavior : MonoBehaviour
{
    public static int recievedPerk = 0;

    public GameObject pMachine;
    public GameObject gun;
    public GameObject healthSoda;
    public GameObject speedSoda;
    public GameObject reloadSoda;

    public TextMeshProUGUI promptTex;

    private float drinkDuration = 3f;
    public static bool isDrinking = false;
    // NEW — soda sound script
    private SodaSound sodaSound;
    
    void Start(){
        healthSoda.SetActive(false);
        speedSoda.SetActive(false);
        reloadSoda.SetActive(false);
        sodaSound = GetComponent<SodaSound>();
    }
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
        // SOUND — open can
        sodaSound?.PlayOpen();
        yield return new WaitForSeconds(0.2f);

        if (recievedPerk == 1){
            healthSoda.SetActive(true);
            sodaSound?.PlayDrink();
            yield return new WaitForSeconds(drinkDuration);
            PerkChecker.hasDoubleHealth = true;
        }
        if (recievedPerk == 2){
            speedSoda.SetActive(true);
            sodaSound?.PlayDrink();
            yield return new WaitForSeconds(drinkDuration);
            PerkChecker.hasFasterMovement = true;
        }
        if (recievedPerk == 3){
            reloadSoda.SetActive(true);
            sodaSound?.PlayDrink();
            yield return new WaitForSeconds(drinkDuration);
            PerkChecker.hasSpeedReload = true;
        }
        // SOUND — drop can
        sodaSound?.PlayDrop();
        gun.SetActive(true);
        pMachine.SetActive(true);
        promptTex.enabled = true;
        GunScriptBase.isReloading = false;
        recievedPerk = 0;

        healthSoda.SetActive(false);
        speedSoda.SetActive(false);
        reloadSoda.SetActive(false);

        isDrinking = false;
    }
}
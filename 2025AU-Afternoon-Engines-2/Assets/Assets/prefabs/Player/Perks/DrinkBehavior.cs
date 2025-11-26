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

    public AudioSource openCan;
    public AudioSource drink;
    public AudioSource dropCan;

    private float drinkDuration = 3f;
    private bool isDrinking = false;

    void Start() {
        healthSoda.SetActive(false);
        speedSoda.SetActive(false);
        reloadSoda.SetActive(false);
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
        openCan.Play();
        yield return new WaitForSeconds(0.2f);
        
        if (recievedPerk == 1) {
            healthSoda.SetActive(true);
            drink.Play();
            yield return new WaitForSeconds(drinkDuration); 
            PerkChecker.hasDoubleHealth = true;
        }
        if (recievedPerk == 2) {
            speedSoda.SetActive(true);
            drink.Play();
            yield return new WaitForSeconds(drinkDuration); 
            PerkChecker.hasFasterMovement = true;
        }
        if (recievedPerk == 3) {
            reloadSoda.SetActive(true);
            drink.Play();
            yield return new WaitForSeconds(drinkDuration); 
            PerkChecker.hasSpeedReload = true;
        }
        dropCan.Play();
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
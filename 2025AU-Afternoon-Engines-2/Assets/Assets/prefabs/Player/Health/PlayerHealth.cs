using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{   
    public float playerHealth = 100 * PerkChecker.HealthPerkMult;
    public GameObject gun;
    public GameObject parent;
    private float regenTime = 4f;
    private float iframes = 0.8f;
    private float maxHealth = 100;

    [Header("Damage Settings")] 
    public float meleeDamage = 34f; // editable in Inspector
    public float hitDamageMultiplier = 1f; // optional balancing multiplier 

    public TextMeshProUGUI healthTex;
    public Image healthBar;
    public Image doubleHealthBar;

    public GameObject loseScreen;

    void Start()
    {
//        Debug.Log("[PlayerHealth] Start — Initial Health: " + playerHealth);
        setHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenTime <= 0f && playerHealth > 0 && playerHealth < maxHealth * PerkChecker.HealthPerkMult) {
            heal();
        }
        else if (playerHealth > maxHealth * PerkChecker.HealthPerkMult) {
            playerHealth -= 1;
        }
        tick();
    }
    private void heal() {
        playerHealth += 0.125f * PerkChecker.HealthPerkMult;
        setHealthUI();
    }

    public void PlayerDamage(float amount)
    {
        float finalDamage = amount * hitDamageMultiplier;
//        Debug.Log("[PlayerHealth] PlayerDamage(" + finalDamage + ") called");

        if (playerHealth > 0 && iframes <= 0)
        {
            playerHealth -= finalDamage;
            regenTime = 6f;
            iframes = 0.8f;
            setHealthUI();
        }
    }

    private void tick() {
        if (regenTime > 0)
            regenTime -= Time.deltaTime;
        if (iframes > 0)
            iframes -= Time.deltaTime;
        if (playerHealth <= 0) {
            // LOSE SOUND TRIGGER ADD BY THOMAS
            FindFirstObjectByType<GameOutcomeSound>()?.PlayLose();


            playerHealth = 0;
//            Debug.Log("[PlayerHealth] PLAYER DIED!");
            gun.SetActive(false);
            parent.SetActive(false);

            if (loseScreen != null)
            {
                loseScreen.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }
    void setHealthUI() {
        healthTex.text = Mathf.Floor(playerHealth).ToString();
        healthBar.fillAmount = playerHealth / 100;
        doubleHealthBar.fillAmount = (playerHealth - 100) / 100;
    }
}

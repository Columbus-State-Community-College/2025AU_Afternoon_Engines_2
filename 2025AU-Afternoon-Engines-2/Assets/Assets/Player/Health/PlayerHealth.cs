using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{   
    public float playerHealth = 100 * PerkChecker.HealthPerkMult;
    public GameObject gun;
    public GameObject parent;
    private float regenTime = 4f;
    private float iframes = 0.8f;

     [Header("Damage Settings")] // added by Thomas 
    public float meleeDamage = 34f; // editable in Inspector 
    public float hitDamageMultiplier = 1f; // optional balancing multiplier 
   
    public TextMeshProUGUI healthTex;
    void Start()
    {
        Debug.Log("[PlayerHealth] Start — Initial Health: " + playerHealth); // added by Thomas for testing
        setText();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenTime <= 0f && playerHealth > 0 && playerHealth < 100 * PerkChecker.HealthPerkMult) {
           heal();
        }
        else if (playerHealth > 100 * PerkChecker.HealthPerkMult) {
            playerHealth -= 1;
        }
        tick();

    }
    private void heal() {
            playerHealth += 0.5f * PerkChecker.HealthPerkMult;
            setText();
    }

    // NEW: external zombie damage
    public void PlayerDamage(float amount) // added by Thomas
    {
        float finalDamage = amount * hitDamageMultiplier; // allows balancing 

        Debug.Log("[PlayerHealth] PlayerDamage(" + finalDamage + ") called"); 

        if (playerHealth > 0 && iframes <= 0)
        {
            playerHealth -= finalDamage;
            regenTime = 4f;
            iframes = 0.8f;

            Debug.Log("[PlayerHealth] Took " + finalDamage + " damage — Health now: " + playerHealth); 
            setText();
        }
        else
        {
            Debug.Log("[PlayerHealth] Damage ignored (iFrames active)"); 
        }
    }

    // OLD damage logic — used for trigger-based melee hits
    public void PlayerDamage() {
        if (playerHealth > 0 && iframes <= 0) {

            Debug.Log("[PlayerHealth] Melee trigger hit — damage: " + meleeDamage); // added by Thomas 

            playerHealth -= meleeDamage; // editable in Inspector
            regenTime = 4f;
            iframes = 0.8f;
            setText();
        }  
    }

    private void PlayerDamage_Internal() // added by Thomas
    {
        if (playerHealth > 0 && iframes <= 0)
        {
            Debug.Log("[PlayerHealth] Melee trigger hit — damage: " + meleeDamage); 

            playerHealth -= meleeDamage; // editable in Inspector
            regenTime = 4f;
            iframes = 0.8f;

            setText();
        }
    }

    private void OnTriggerStay(Collider other) {

        Debug.Log("[PlayerHealth] OnTriggerStay detected with: " + other.name); // added by Thomas for testing

        if (other.gameObject.CompareTag("Enemy") && iframes <= 0) {

            Debug.Log("[PlayerHealth] Attempting melee damage"); // added by Thomas for testing


            PlayerDamage_Internal(); // uses meleeDamage
        }
    }
    private void tick() {
        if (regenTime > 0) {
            regenTime -= Time.deltaTime;
        }
        if (iframes > 0) {
            iframes -= Time.deltaTime;
        }
        if (playerHealth <= 0) {

            Debug.Log("[PlayerHealth] PLAYER DIED!"); // added by Thomas for testing

            gun.SetActive(false);
            parent.SetActive(false);
        }
    }
    void setText() {
        healthTex.text = Mathf.Floor(playerHealth).ToString();
    }
    
}

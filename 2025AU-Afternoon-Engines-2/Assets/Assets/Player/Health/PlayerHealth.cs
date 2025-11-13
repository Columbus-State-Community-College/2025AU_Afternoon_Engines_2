using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{   
    public static float playerHealth = 100 * PerkChecker.HealthPerkMult;
    public GameObject gun;
    public GameObject parent;
    private float regenTime = 4f;
    private float iframes = 0.8f;
   
    public TextMeshProUGUI healthTex;
    void Start()
    {
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
    private void PlayerDamage() {
        if (playerHealth > 0 && iframes <= 0) {
            playerHealth -= 34;
            regenTime = 4f;
            iframes = 0.8f;
            setText();
        }  
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Enemy") && iframes <= 0) {
            PlayerDamage();
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
            gun.SetActive(false);
            parent.SetActive(false);
        }
    }
    void setText() {
        healthTex.text = Mathf.Floor(playerHealth).ToString();
    }
    
}

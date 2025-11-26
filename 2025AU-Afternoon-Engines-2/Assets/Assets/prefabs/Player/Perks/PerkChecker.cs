using UnityEngine;
using UnityEngine.UI;

public class PerkChecker : MonoBehaviour
{
    public static bool hasSpeedReload = false;
    public static bool hasFasterMovement = false;
    public static bool hasDoubleHealth = false;
    public static float MovementPerkSpeed = 1f;
    public static float HealthPerkMult = 1f;

    public Image healthPerkIcon;
    public Image speedPerkIcon;
    public Image reloadPerkIcon;

    void Update()
    {
        if (hasFasterMovement == true) {
            MovementPerkSpeed = 1.2f;
            speedPerkIcon.enabled = true;
        }
        else {
            MovementPerkSpeed = 1f;
            speedPerkIcon.enabled = false;
        }
        if (hasDoubleHealth == true) {
            HealthPerkMult = 2f;
            healthPerkIcon.enabled = true;
        }
        else {
            HealthPerkMult = 1f;
            healthPerkIcon.enabled = false;
        }
        if (hasSpeedReload == true) {
            reloadPerkIcon.enabled = true;
        }
        else {
            reloadPerkIcon.enabled = false;
        }
        
    }
}

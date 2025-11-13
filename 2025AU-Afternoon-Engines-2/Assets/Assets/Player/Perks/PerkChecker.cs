using UnityEngine;

public class PerkChecker : MonoBehaviour
{
    public static bool hasSpeedReload = false;
    public static bool hasFasterMovement = false;
    public static bool hasDoubleHealth = false;
    public static float MovementPerkSpeed = 1f;
    public static float HealthPerkMult = 1f;


    void Update()
    {
        if (hasFasterMovement == true) {
            MovementPerkSpeed = 1.2f;
        }
        else {
            MovementPerkSpeed = 1f;
        }
        if (hasDoubleHealth == true) {
            HealthPerkMult = 2f;
        }
    }
}

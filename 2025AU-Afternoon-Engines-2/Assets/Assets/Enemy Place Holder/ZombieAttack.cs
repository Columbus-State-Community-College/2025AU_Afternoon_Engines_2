using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRange = 1.8f; // how close zombie must be to attack
    public float attackStopDistance = 1.2f; // how close zombie can get before stopping
    public float attackCooldown = 3f; // time between attacks

    public float attackDamage = 10f; // editable in Inspector

    [Header("Debug Settings")]
    public float logInterval = 60f;  // controls how often spammy logs print

    private float logTimer = 0f;

    private Transform player; // assigned automatically at runtime
    private float lastAttackTime;

    void Start()
    {
        // Attempt to find player by tag
        player = GameObject.FindWithTag("Player").transform;
        Debug.Log("[ZombieAttack] Start() — Player assigned: " + player.name);
    }

    void Update()
    {
        // If player reference is not ready, keep searching
        if (player == null)
        {
            Debug.LogWarning("[ZombieAttack] Player reference is NULL");
            return;
        }
    
        float distance = Vector3.Distance(transform.position, player.position);
        // Debug for seeing range checks

        // Update timer for throttled logs
        logTimer += Time.deltaTime;

        // Throttled distance/readout logs
        if (logTimer >= logInterval)
        {
            Debug.Log("[ZombieAttack] Distance to player: " + distance + " (Range=" + attackRange + ")");
            logTimer = 0f;
        }

        // Check if within attack range
        if (distance <= attackRange)
        {
            // Throttled "inside attack range" log
            if (logTimer == 0f)
            {
                Debug.Log("[ZombieAttack] Player is inside attack range");
            }

            // Check cooldown
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                Debug.Log("[ZombieAttack] ATTACK EXECUTED!");

                PlayerHealth health = player.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    Debug.Log("[ZombieAttack] Calling PlayerHealth.PlayerDamage(" + attackDamage + ")");
                    health.PlayerDamage(attackDamage); // uses Inspector damage
                }
                else
                {
                    Debug.LogWarning("[ZombieAttack] PlayerHealth script NOT FOUND");
                }
            }
            else
            {
                float cd = (lastAttackTime + attackCooldown) - Time.time;

                // Throttled cooldown log
                if (logTimer == 0f)
                {
                    Debug.Log("[ZombieAttack] Attack on cooldown: " + cd.ToString("F2") + " seconds remaining");
                }
            }
        }
    }
}

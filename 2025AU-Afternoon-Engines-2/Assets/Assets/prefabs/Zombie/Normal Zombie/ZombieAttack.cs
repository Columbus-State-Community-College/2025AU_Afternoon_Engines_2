using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackRange = 1.8f; // how close zombie must be to attack
    public float attackStopDistance = 1.2f; // how close zombie can get before stopping
    public float attackCooldown = 3f; // time between attacks
    public float attackDamage = 10f; // editable in Inspector

    [Header("Debug Settings")]
    public float logInterval = 30f; // controls how often spammy logs print

    private float logTimer = 0f;

    private Transform player; // assigned automatically at runtime
    private PlayerHealth playerHealth;   
    private float lastAttackTime;

    void Start()
    {
        // Find the PlayerObject (tagged "Player")
        Transform taggedPlayer = GameObject.FindWithTag("Player").transform;

        // PlayerHealth is on the parent (Player)
        playerHealth = taggedPlayer.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
        {
//            Debug.LogError("[ZombieAttack] ERROR — Could not find PlayerHealth on Player or parent!");
            return;
        }

        // Attack THIS transform (the Player that holds PlayerHealth)
        player = playerHealth.transform;
    }

    void Update()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        logTimer += Time.deltaTime;
        if (logTimer >= logInterval)
        {
            // Debug.Log("[ZombieAttack] Distance: " + distance);
            logTimer = 0f;
        }

        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

//                 Debug.Log("[ZombieAttack] ATTACK EXECUTED!");
                playerHealth.PlayerDamage(attackDamage);
            }
        }
    }
}

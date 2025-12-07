using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float attackRange = 1.8f;
    public float attackStopDistance = 1.2f;
    public float attackCooldown = 3f;

    public float attackDamage = 22f;

    [Header("Debug Settings")]
    public float logInterval = 30f;

    private float logTimer = 0f;
    private Transform player;           // The actual transform we attack
    private PlayerHealth playerHealth;  // Cached PlayerHealth
    private float lastAttackTime;

    void Start()
    {
        // Find PlayerObject (tagged "Player")
        Transform taggedPlayer = GameObject.FindWithTag("Player").transform;

        // PlayerHealth is on a parent (Player), so walk up
        playerHealth = taggedPlayer.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
        {
//            Debug.LogError("[BossAttack] ERROR — Could not find PlayerHealth on Player or its parents!");
            return;
        }

        player = playerHealth.transform;

//        Debug.Log("[BossAttack] Start() — Player assigned: " + player.name);
    }

    void Update()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        logTimer += Time.deltaTime;
        if (logTimer >= logInterval)
        {
//            Debug.Log("[BossAttack] Distance to player: " + distance + " (Range=" + attackRange + ")");
            logTimer = 0f;
        }

        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
//                Debug.Log("[BossAttack] ATTACK EXECUTED!");

                playerHealth.PlayerDamage(attackDamage);
            }
        }
    }
}

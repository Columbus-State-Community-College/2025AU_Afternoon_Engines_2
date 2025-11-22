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
    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        // Attempt to find player by tag
        player = GameObject.FindWithTag("Player").transform;
//      Debug.Log("[BossAttack] Start() — Player assigned: " + player.name);
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("[BossAttack] Player reference is NULL");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        logTimer += Time.deltaTime;
        if (logTimer >= logInterval)
        {
            Debug.Log("[BossAttack] Distance to player: " + distance + " (Range=" + attackRange + ")");
            logTimer = 0f;
        }

        if (distance <= attackRange)
        {
            if (logTimer == 0f)
                Debug.Log("[BossAttack] Player is inside attack range");

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Debug.Log("[BossAttack] ATTACK EXECUTED!");

                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    Debug.Log("[BossAttack] Calling PlayerHealth.PlayerDamage(" + attackDamage + ")");
                    health.PlayerDamage(attackDamage);
                }
                else
                {
                    Debug.LogWarning("[BossAttack] PlayerHealth script NOT FOUND");
                }
            }
            else
            {
                float cd = (lastAttackTime + attackCooldown) - Time.time;

                if (logTimer == 0f)
                    Debug.Log("[BossAttack] Attack on cooldown: " + cd.ToString("F2") + " seconds remaining");
            }
        }
    }
}

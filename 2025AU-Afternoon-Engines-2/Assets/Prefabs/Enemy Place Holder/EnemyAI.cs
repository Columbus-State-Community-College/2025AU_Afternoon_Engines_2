using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Target Settings")]
    public string playerTag = "Player";
    private Transform player;

    [Header("AI Settings")]
    public float detectionRange = 15f;
    public float chaseSpeed = 3.5f;
    public float idleSpeed = 0f;

    [Header("Bounce Settings")]
    public float bounceForce = 2f;
    public float disableTime = 0.5f;

    [Header("Debug Settings")]
    public float logInterval = 60f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private float logTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = idleSpeed;

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("[EnemyAI] Found player: " + player.name);
        }
        else
        {
            Debug.LogWarning("[EnemyAI] No player found! Check Player tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        logTimer += Time.deltaTime;
        if (logTimer >= logInterval)
        {
            Debug.Log("[EnemyAI] Distance to player: " + distance.ToString("F2"));
            logTimer = 0f;
        }

        if (!agent.enabled || !agent.isOnNavMesh)
            return;

        if (distance <= detectionRange)
        {
            agent.speed = chaseSpeed;

            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            if (agent.enabled && agent.isOnNavMesh)
                agent.ResetPath();

            agent.speed = idleSpeed;
        }
    }

    // === Bounce behavior when colliding with player ===
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            Vector3 bounceDir = (transform.position - collision.transform.position).normalized;
            agent.enabled = false;

            StartCoroutine(SmallPushBack(bounceDir));
            Debug.Log("[EnemyAI] Light pushback from player!");
        }
    }

    private IEnumerator SmallPushBack(Vector3 direction)
    {
        float pushDistance = 1.5f;
        float pushSpeed = 5f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + (direction * pushDistance);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * pushSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(disableTime);

        //  re-enable AI NavMeshAgent 
        if (agent != null)
            agent.enabled = true;
    }
}

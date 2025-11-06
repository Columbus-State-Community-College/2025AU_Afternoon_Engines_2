using UnityEngine;
using UnityEngine.AI;   
using System.Collections;

public class ZombieHitbox : MonoBehaviour
{
    [Header("Score Values")]
    public int pointsForHit = 10;
    public int pointsForKill = 50;
    public bool isHead = false;

    [Header("References")]
    public ZombieHealth parentZombie; // assign in inspector

    [Header("Hit Reaction")]
    public float knockbackForce = 3f;   // tweak this in inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;

        Bullet bullet = other.GetComponent<Bullet>();
        int damageAmount = bullet != null ? (int)bullet.damage : 20;

        ApplyDamage(damageAmount);
        ApplyKnockback(other.transform);

        Destroy(other.gameObject);
    }

    private void ApplyKnockback(Transform bullet)
    {
        Rigidbody rb = parentZombie.GetComponent<Rigidbody>();
        NavMeshAgent agent = parentZombie.GetComponent<NavMeshAgent>();

        if (rb == null) return;

        // Temporarily disable AI so physics can push it
        if (agent != null)
            agent.enabled = false;

        Vector3 direction = (parentZombie.transform.position - bullet.position).normalized;
        direction.y = 0;

        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        // Re-enable AI after short delay
        StartCoroutine(ReenableAgent(agent));
    }

    private IEnumerator ReenableAgent(NavMeshAgent agent)
    {
        if (agent == null) yield break;

        yield return new WaitForSeconds(0.1f); // small pause
        agent.enabled = true;
    }

    private void ApplyDamage(int damage)
    {
        parentZombie.TakeDamage(damage);

        int points = isHead ? pointsForHit * 2 : pointsForHit;
        ScoreManager.instance.AddPoints(points);

        if (parentZombie.IsDead())
            ScoreManager.instance.AddPoints(pointsForKill);

        Debug.Log($"[ZombieHitbox] {(isHead ? "HEAD" : "BODY")} hit → {damage} damage");
    }
}

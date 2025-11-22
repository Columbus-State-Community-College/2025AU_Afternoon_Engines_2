using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossHitbox : MonoBehaviour
{
    [Header("Score Values")]
    public int pointsForHit = 100;
    public int pointsForKill = 200;
    public bool isHead = false;

    [Header("Hit Reaction")]
    public float knockbackForce = 2f;

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.01f;

    private bool recentlyHit = false;
    private BossHealth parentZombie;

    private void Awake()
    {
        parentZombie = GetComponentInParent<BossHealth>();
        if (parentZombie == null)
            Debug.LogError($"{name} is missing BossHealth in parent");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Bullet")) return;

        Debug.Log("[BossHitbox] Bullet collision registered");

        if (recentlyHit) return;

        recentlyHit = true;
        StartCoroutine(ResetHitFlag());

        Bullet bullet = collision.collider.GetComponent<Bullet>();
        int damageAmount = bullet != null ? (int)bullet.damage : 20;

        ApplyDamage(damageAmount);
        ApplyKnockback(collision.collider.transform);

        Destroy(collision.gameObject);
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(hitCooldown);
        recentlyHit = false;
        Debug.Log("[BossHitbox] Hit cooldown reset");
    }

    private void ApplyKnockback(Transform bullet)
    {
        Rigidbody rb = parentZombie.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("[BossHitbox] No Rigidbody on boss!");
            return;
        }

        Vector3 direction = (parentZombie.transform.position - bullet.position).normalized;
        direction.y = 0;

        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        Debug.Log("[BossHitbox] Knockback applied");
    }

    private void ApplyDamage(int damage)
    {
        bool wasDead = parentZombie.IsDead();

        Debug.Log($"[BossHitbox] Hit registered on {(isHead ? "HEAD" : "BODY")} (Damage: {damage})");

        parentZombie.TakeDamage(damage);

        ZombieSound zs = parentZombie.GetComponent<ZombieSound>();
        if (zs != null) zs.PlayHitSound();

        if (parentZombie.IsDead() && !wasDead)
        {
            Debug.Log("[BossHitbox] Boss killed — adding kill points");
            ScoreManager.instance.AddPoints(pointsForKill);
            if (zs != null) zs.StopMoan();
        }
        else if (!parentZombie.IsDead())
        {
            Debug.Log("[BossHitbox] Boss hit — adding hit points");
            ScoreManager.instance.AddPoints(pointsForHit);
        }
    }
}

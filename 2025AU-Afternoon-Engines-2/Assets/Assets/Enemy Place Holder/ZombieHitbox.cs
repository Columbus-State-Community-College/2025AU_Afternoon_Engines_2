using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieHitbox : MonoBehaviour
{
    [Header("Score Values")]
    public int pointsForHit = 10;
    public int pointsForKill = 50;
    public bool isHead = false;

    [Header("Hit Reaction")]
    public float knockbackForce = 3f; // tweak this in inspector

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.05f; // prevents double hits

    private bool recentlyHit = false;
    private ZombieHealth parentZombie;

    private void Awake()
    {
        parentZombie = GetComponentInParent<ZombieHealth>();
        if (parentZombie == null)
            Debug.LogError($"{name} is missing ZombieHealth in parent");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // only respond to bullets
        if (!collision.collider.CompareTag("Bullet")) return;

        // prevents 2 hitboxes from triggering in the same frame
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
    }

    private void ApplyKnockback(Transform bullet)
    {
        Rigidbody rb = parentZombie.GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 direction = (parentZombie.transform.position - bullet.position).normalized;
        direction.y = 0;

        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        // dont disable NavMesh, it was causing the enemies to not die
    }

    private void ApplyDamage(int damage)
    {
        bool wasDead = parentZombie.IsDead();
        parentZombie.TakeDamage(damage);

        // play hit sound
        ZombieSound zs = parentZombie.GetComponent<ZombieSound>();
        if (zs != null)
            zs.PlayHitSound();

        // points if killed this frame (not before)
        if (parentZombie.IsDead() && !wasDead)
        {
            ScoreManager.instance.AddPoints(pointsForKill);

            // stop moaning if dead
            if (zs != null)
                zs.StopMoan();
        }
        else if (!parentZombie.IsDead() && !wasDead)
        {
            ScoreManager.instance.AddPoints(pointsForHit);
        }
    }
}

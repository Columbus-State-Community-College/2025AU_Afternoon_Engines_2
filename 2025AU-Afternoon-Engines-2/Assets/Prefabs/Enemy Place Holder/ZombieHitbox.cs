using UnityEngine;

public class ZombieHitbox : MonoBehaviour
{
    [Header("Score Values")]
    public int pointsForHit = 10;
    public int pointsForKill = 50;
    public bool isHead = false;

    [Header("References")]
    public ZombieHealth parentZombie; // assign in Inspector

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    private void HandleHit(GameObject obj)
    {
        // Check for bullet collision
        if (obj.CompareTag("Bullet"))
        {
            // Try to get Bullet script
            Bullet bullet = obj.GetComponent<Bullet>();

            if (bullet != null)
            {
                ApplyDamage((int)bullet.damage, "[Bullet]");
            }
            else
            {
                Debug.LogWarning("[ZombieHitbox] Bullet hit, but no Bullet script found!");
                ApplyDamage(20, "[Unknown Projectile]");
            }

            // Destroy bullet after hit (safe cleanup)
            Destroy(obj);
        }
    }

    private void ApplyDamage(int damage, string source)
    {
        parentZombie.TakeDamage(damage);

        int points = isHead ? pointsForHit * 2 : pointsForHit;
        ScoreManager.instance.AddPoints(points);

        if (parentZombie.IsDead())
        {
            ScoreManager.instance.AddPoints(pointsForKill);
        }

        Debug.Log($"[ZombieHitbox] {source} → {(isHead ? "Head" : "Body")} hit for {damage} dmg.");
    }
}

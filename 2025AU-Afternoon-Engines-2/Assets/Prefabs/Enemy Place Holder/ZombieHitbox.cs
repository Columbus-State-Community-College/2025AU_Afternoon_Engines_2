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
    public float knockbackForce = 3f;   // tweak this in inspector

    private ZombieHealth parentZombie;

    private void Awake()
    {
        parentZombie = GetComponentInParent<ZombieHealth>();
        if (parentZombie == null)
            Debug.LogError($"{name} is missing ZombieHealth in parent");
    }

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

        // temp scoring fix
        if (parentZombie.IsDead() && !wasDead)
        {
            ScoreManager.instance.AddPoints(pointsForKill);
        }

        else if (!parentZombie.IsDead() && !wasDead)
        {
            ScoreManager.instance.AddPoints(pointsForHit);
        }

        // if zombie is dead, stop looping sound
        if (parentZombie.IsDead())
            zs.StopMoan();
    }

}

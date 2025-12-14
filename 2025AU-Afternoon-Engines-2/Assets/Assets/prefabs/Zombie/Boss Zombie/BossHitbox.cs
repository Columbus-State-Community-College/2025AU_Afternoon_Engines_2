using UnityEngine;
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
        {
//            Debug.LogError($"{name} is missing BossHealth in parent!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet")) return;
        if (recentlyHit) return;

        recentlyHit = true;
        StartCoroutine(ResetHitFlag());

        Bullet bullet = other.GetComponent<Bullet>();
        int damageAmount = bullet != null ? (int)bullet.damage : 20;

//        Debug.Log($"[BossHitbox] {(isHead ? "HEADSHOT" : "BODYSHOT")} for {damageAmount} dmg");

        ApplyDamage(damageAmount);
        ApplyKnockback();
        Destroy(other.gameObject);
    }

    private IEnumerator ResetHitFlag()
    {
        yield return new WaitForSeconds(hitCooldown);
        recentlyHit = false;
    }

    private void ApplyKnockback()
    {
        Rigidbody rb = parentZombie.GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.AddForce(Vector3.back * knockbackForce, ForceMode.Impulse);
    }

    private void ApplyDamage(int damage)
    {
        bool wasDead = parentZombie.IsDead();
        parentZombie.TakeDamage(damage);

        BossZombieSound zs = parentZombie.GetComponent<BossZombieSound>();
        if (zs != null) zs.PlayHitSound();

        if (parentZombie.IsDead() && !wasDead)
        {
            ScoreManager.instance.AddPoints(pointsForKill);
            if (zs != null) zs.StopMoan();
        }
        else if (!parentZombie.IsDead())
        {
            ScoreManager.instance.AddPoints(pointsForHit);
        }
    }
}

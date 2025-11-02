using UnityEngine;

public class ZombieHitbox : MonoBehaviour
{
    [Header("Score Values")]
    public int pointsForHit = 10;
    public int pointsForKill = 50;
    public bool isHead = false;

    [Header("References")]
    public ZombieHealth parentZombie; // assign in Inspector

    public void TakeDamage(int damage)
    {
        parentZombie.TakeDamage(damage);

        int points = isHead ? pointsForHit * 2 : pointsForHit;
        ScoreManager.instance.AddPoints(points);

        if (parentZombie.IsDead())
        {
            ScoreManager.instance.AddPoints(pointsForKill);
            Debug.Log($"[ZombieHitbox] {gameObject.name}: Killed (+{pointsForKill} pts)");
        }
        else
        {
            Debug.Log($"[ZombieHitbox] {gameObject.name}: Hit for {damage} dmg (+{points} pts)");
        }
    }

    // Temporary click-to-damage test
    private void OnMouseDown()
    {
        if (parentZombie == null)
        {
            Debug.LogWarning("[ZombieHitbox] No parentZombie reference!");
            return;
        }

        int damage = isHead ? 50 : 25;
        TakeDamage(damage);
    }
}

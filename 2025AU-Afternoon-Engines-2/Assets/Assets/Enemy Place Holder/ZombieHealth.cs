using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    [Header("Zombie Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[ZombieHealth] {gameObject.name} spawned with {maxHealth} HP");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"[ZombieHealth] {gameObject.name} took {damage} dmg ({currentHealth}/{maxHealth})");

        if (currentHealth == 0)
            Die();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die()
    {
        Debug.Log($"[ZombieHealth] {gameObject.name} died!");

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // stop any zombie sounds
        ZombieSound zs = GetComponent<ZombieSound>();
        if (zs != null)
        {
            zs.StopMoan();
        }

        // play death SFX if we add one
        //SoundManager.instance?.PlaySFX("Zombie death SFX name goes here");

        Destroy(gameObject, 2f); // delay allows sound to play
    }
}

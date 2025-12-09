using UnityEngine;
using UnityEngine.AI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health Settings")]
    public int maxHealth = 275;   // 2.75x stronger
    private int currentHealth;

    [HideInInspector] public waveSpawner waveSpawner;
    [HideInInspector] public Wave myWave;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[BossHealth] {gameObject.name} spawned with {maxHealth} HP");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"[BossHealth] {gameObject.name} took {damage} dmg ({currentHealth}/{maxHealth})");

        if (currentHealth == 0)
            Die();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die()
    {
        Debug.Log($"[BossHealth] {gameObject.name} died!");

        if (isDead) return;
        isDead = true;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        BossZombieSound zs = GetComponent<BossZombieSound>();
        if (zs != null)
        {
            zs.StopMoan();
        }

        if (myWave != null)
        {
            myWave.enemiesLeft--;
        }

        Destroy(gameObject, 2f);
    }
}

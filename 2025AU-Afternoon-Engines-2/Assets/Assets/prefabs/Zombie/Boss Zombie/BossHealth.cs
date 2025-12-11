using UnityEngine;
using UnityEngine.AI;

public class BossHealth : MonoBehaviour
{
    [Header("Boss Health Settings")]
    public int maxHealth = 275; 
    private int currentHealth;

    [HideInInspector] public waveSpawner waveSpawner;
    [HideInInspector] public Wave myWave;

    private bool isDead = false;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
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

        // Disable NavMesh movement
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        // Disable all colliders (no blocking)
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Stop boss sounds
        BossZombieSound zs = GetComponent<BossZombieSound>();
        if (zs != null)
            zs.StopMoan();

        // Update wave count
        if (myWave != null)
            myWave.enemiesLeft--;

        
        if (anim != null)
            anim.SetTrigger("Die");

        //  DESPAWN TIME 
        Destroy(gameObject, 0.1f);
    }
}

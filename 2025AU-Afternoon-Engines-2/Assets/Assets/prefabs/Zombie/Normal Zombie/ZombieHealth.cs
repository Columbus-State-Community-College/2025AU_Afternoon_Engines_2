using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    [Header("Zombie Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [HideInInspector] public waveSpawner waveSpawner;
    [HideInInspector] public Wave myWave;

    private bool isDead = false;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
//        Debug.Log($"[ZombieHealth] {gameObject.name} spawned with {maxHealth} HP");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

//        Debug.Log($"[ZombieHealth] {gameObject.name} took {damage} dmg ({currentHealth}/{maxHealth})");

        if (currentHealth == 0)
            Die();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die()
    {
//        Debug.Log($"[ZombieHealth] {gameObject.name} died!");

        if (isDead) return;
        isDead = true;

        // Disable NavMesh so it stops moving
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // Disable colliders so it no longer blocks player or bullets
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Stop zombie sounds
        ZombieSound zs = GetComponent<ZombieSound>();
        if (zs != null) zs.StopMoan();

        // Update the wave counter
        if (myWave != null)
            myWave.enemiesLeft--;

        // Trigger death animation 
        if (anim != null)
            anim.SetTrigger("Die");

        //  DESPAWN TIME 
        Destroy(gameObject, 0.1f); 
    }
}

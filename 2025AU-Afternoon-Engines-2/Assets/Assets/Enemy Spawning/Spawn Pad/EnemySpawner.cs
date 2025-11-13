using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float enemySpawninterval = 10f;

    private float timer;

    
    void Start()
    {
        timer = enemySpawninterval; // start the timer
    }

    void Update()
    {
        timer = timer - Time.deltaTime;

        if (timer <= 0f)
        {
            spawnEnemy();
            timer = enemySpawninterval;
        }
    }

    private void spawnEnemy()
    {
        Instantiate(EnemyPrefab, gameObject.transform.position, Quaternion.identity);
    }
}

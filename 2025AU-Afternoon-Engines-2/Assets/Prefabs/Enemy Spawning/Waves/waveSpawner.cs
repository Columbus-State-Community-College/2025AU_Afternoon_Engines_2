using UnityEngine;
using System.Collections;

public class waveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private Transform[] spawnPoints;

    public Wave[] waves;

    private int currentWaveIndex = 0;
    private bool spawning = false;

    private void Start()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    private void Update()
    {
        if (spawning) return;
        if (currentWaveIndex >= waves.Length) return; // stop after last wave

        countdown = countdown - Time.deltaTime;

        if (countdown <= 0)
        {
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        spawning = true;

        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.enemies.Length; i++)
        {
            if (spawnPoints.Length == 0)
            {
                Debug.LogError("No spawn points assigned");
                spawning = false;
                yield break;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(currentWave.enemies[i], spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(currentWave.timeToNextEnemy);
        }

        yield return new WaitForSeconds(currentWave.timeToNextWave);

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves complete");
            spawning = false;
            yield break;
        }

        countdown = waves[currentWaveIndex].timeToNextWave;
        spawning = false;
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}
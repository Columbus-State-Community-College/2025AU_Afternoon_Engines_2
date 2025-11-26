using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class waveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private Transform[] spawnPoints;

    public Wave[] waves;

    public int currentWaveIndex = 0;
    private bool spawning = false;

    private bool readyToCountDown;

    [Header("UI Ties")]
    public TMP_Text waveText;
    public TMP_Text enemiesLeftText;
    public TMP_Text timeUntilNextWaveText;
    public GameObject winScreen;

    [Header("To Disable Gun When Player Wins")]
    public GameObject gun;
    public GameObject parent;

    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    private void Update()
    {
        // wave counter
        waveText.text = "Wave: " + (currentWaveIndex + 1);

        // enemy counter
        if (currentWaveIndex < waves.Length)
            enemiesLeftText.text = "Enemies Left: " + waves[currentWaveIndex].enemiesLeft;
        else
            enemiesLeftText.text = "Enemies Left: 0";

        // between wave countdown
        bool waveOver = false;
        if (currentWaveIndex < waves.Length)
        {
            waveOver = !spawning && waves[currentWaveIndex].enemiesLeft <= 0;
        }

        if (readyToCountDown && !spawning)
        {
            timeUntilNextWaveText.gameObject.SetActive(true);
            timeUntilNextWaveText.text = "Next Wave In: " + countdown.ToString("F1") + "s";

            countdown -= Time.deltaTime;
        }
        else
        {
            timeUntilNextWaveText.gameObject.SetActive(false);
        }

        if (spawning) return;

        if (currentWaveIndex >= waves.Length)
        {
            return;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;
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
                spawning = false;
                yield break;
            }

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // refernce to spawn enemy
            Enemy newEnemy = Instantiate(currentWave.enemies[i], spawnPoint.position, spawnPoint.rotation);

            ZombieHealth zh = newEnemy.GetComponent<ZombieHealth>();
            if (zh != null)
            {
                zh.waveSpawner = this;
                zh.myWave = currentWave;
            }

            BossHealth bh = newEnemy.GetComponent<BossHealth>();
            if (bh != null)
            {
                bh.waveSpawner = this;
                bh.myWave = currentWave;
            }

            // play zombie sound
            ZombieSound zs = newEnemy.GetComponent<ZombieSound>();
            if (zs != null)
            {
                zs.PlayMoanLoop();
            }

            yield return new WaitForSeconds(currentWave.timeToNextEnemy);
        }

        while (currentWave.enemiesLeft > 0)
            yield return null;

        currentWaveIndex++;

        if (currentWaveIndex < waves.Length)
        {
            countdown = waves[currentWaveIndex].timeToNextWave;
            readyToCountDown = true;
        }

        if (currentWaveIndex >= waves.Length)
        {
            if (winScreen != null)
                winScreen.SetActive(true);

            if (gun != null) gun.SetActive(false);
            if (parent != null) parent.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
        }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float spawnIntervalDecrement = 0.05f;
    [SerializeField] private float baseSpawnCount = 1.5f;
    [SerializeField] private float spawnCountIncrement = 0.5f;
    [SerializeField] private float maxWaveDuration = 30f;
    [SerializeField] private float waveCutoffDuration = 3f;

    public float WaveTimer { get { return waveTimer; } }
    private float waveTimer = 0;

    private float spawnInterval = 0;
    private int spawnCount = 0;
    private float rawSpawnCount = 0;
    private int enemiesRemaining = 0;

    private YieldInstruction spawnInstruction;

    void Start()
    {
        spawnInterval = baseSpawnInterval;
        rawSpawnCount = baseSpawnCount;
        spawnInstruction = new WaitForSeconds(spawnInterval);
        spawnCount = Mathf.FloorToInt(rawSpawnCount);
    }

    void Update()
    {
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            // Spawn wave
            StartCoroutine(SpawnWave());
            waveTimer = maxWaveDuration;
            enemiesRemaining = spawnCount;
        }
    }

    public void RemoveEnemy()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0 && waveTimer > waveCutoffDuration)
        {
            waveTimer = waveCutoffDuration;
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < rawSpawnCount; i++)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            }

            yield return spawnInstruction;
        }

        UpdateWave();
    }

    private void UpdateWave()
    {
        // Increase difficulty
        spawnInterval = Mathf.Clamp(spawnInterval - spawnIntervalDecrement, 0.1f, baseSpawnInterval);
        rawSpawnCount += spawnCountIncrement;

        // Update wave data
        spawnInstruction = new WaitForSeconds(spawnInterval);
        spawnCount = Mathf.FloorToInt(rawSpawnCount);
    }
}

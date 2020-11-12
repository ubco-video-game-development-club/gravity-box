using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable] public class OnTimerChangedEvent : UnityEvent<int> { }

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float spawnIntervalDecrement = 0.05f;
    [SerializeField] private float baseSpawnCount = 1.5f;
    [SerializeField] private float spawnCountIncrement = 0.5f;
    [SerializeField] private int maxWaveDuration = 30;
    [SerializeField] private int startDelay = 3;
    [SerializeField] private int waveCutoffDuration = 3;
    [SerializeField] private OnTimerChangedEvent onTimerChanged;

    public int WaveTimer
    {
        get { return _waveTimer; }
        set
        {
            _waveTimer = value;
            if (_waveTimer <= 0)
            {
                StartCoroutine(SpawnWave());
                _waveTimer = maxWaveDuration;
                enemiesRemaining = spawnCount;
            }

            onTimerChanged.Invoke(_waveTimer);

            StopCoroutine(TickWaveTimer());
            StartCoroutine(TickWaveTimer());
        }
    }
    private int _waveTimer;

    private float spawnInterval = 0;
    private int spawnCount = 0;
    private float rawSpawnCount = 0;
    private int enemiesRemaining = 0;

    private YieldInstruction spawnInstruction;
    private YieldInstruction tickInstruction;

    void Awake()
    {
        onTimerChanged = new OnTimerChangedEvent();

        spawnInterval = baseSpawnInterval;
        rawSpawnCount = baseSpawnCount;
        spawnInstruction = new WaitForSeconds(spawnInterval);
        spawnCount = Mathf.FloorToInt(rawSpawnCount);
        tickInstruction = new WaitForSeconds(1);
    }

    void Start()
    {
        WaveTimer = startDelay;
    }

    public void RemoveEnemy()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0 && WaveTimer > waveCutoffDuration)
        {
            WaveTimer = waveCutoffDuration;
        }
    }

    private IEnumerator TickWaveTimer()
    {
        yield return new WaitForSeconds(1);
        WaveTimer--;
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

    public void AddTimerChangedListener(UnityAction<int> call) 
    {
        onTimerChanged.AddListener(call);
    }

    public void RemoveTimerChangedListener(UnityAction<int> call) 
    {
        onTimerChanged.RemoveListener(call);
    }
}

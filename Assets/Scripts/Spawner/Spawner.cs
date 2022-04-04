using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnModes
{
    Fixed,
    Random
}

public class Spawner : MonoBehaviour
{
    public static System.Action OnWaveCompleted;

    [Header("Settings")]
    [SerializeField] private SpawnModes spawnMode = SpawnModes.Fixed;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private float delayBetweenWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    [Header("Poolers")]
    [SerializeField] private ObjectPooler enemyWave10Pooler;
    [SerializeField] private ObjectPooler enemyWave20Pooler;
    [SerializeField] private ObjectPooler enemyWave30Pooler;
    [SerializeField] private ObjectPooler enemyWave40Pooler;
    [SerializeField] private ObjectPooler enemyWave50Pooler;

    private float _spawnTimer;
    private int _enemiesSpawned;
    private int _enemiesRemaining;
    private bool _waveSpawned = false;
    private Enemy _lastEnemyRecorded;

    private Waypoint _waypoint;

    private void Start()
    {
        _waypoint = GetComponent<Waypoint>();
        _enemiesRemaining = enemyCount;
    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer = GetSpawnDelay();
            if (_enemiesSpawned < enemyCount)
            {
                _enemiesSpawned++;
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject newInstance = GetPooler().GetInstanceFromPool();
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.Waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;

        newInstance.SetActive(true);
    }

    private ObjectPooler GetPooler()
    {
        int currentWave = LevelManager.Instance.CurrentWave;
        int currentWaveDivided = Convert.ToInt32(Mathf.Ceil(currentWave / 3f));
        Debug.Log(currentWaveDivided);

        switch (currentWaveDivided)
        {
            case 1:
                return enemyWave10Pooler;
            case 2:
                return enemyWave20Pooler;
            case 3:
                return enemyWave30Pooler;
            case 4:
                return enemyWave40Pooler;
            default:
                return enemyWave50Pooler;
        }
    }

    private float GetSpawnDelay()
    {
        float delay = 0f;

        if (spawnMode == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }

    private float GetRandomDelay()
    {
        float randomTimer = UnityEngine.Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        _enemiesRemaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
        _waveSpawned = false;
        _lastEnemyRecorded = null;
    }

    private void RecordEnemy(Enemy enemy)
    {
        if (_lastEnemyRecorded == null || _lastEnemyRecorded != enemy)
        {
            _enemiesRemaining--;
            _lastEnemyRecorded = enemy;
        }

        if (_enemiesRemaining <= 0 && !_waveSpawned)
        {
            OnWaveCompleted?.Invoke();
            StartCoroutine(NextWave());
            _waveSpawned = true;
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}

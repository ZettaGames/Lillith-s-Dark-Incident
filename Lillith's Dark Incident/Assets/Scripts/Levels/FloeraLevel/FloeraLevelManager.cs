using System.Collections;
using UnityEngine;

public class FloeraLevelManager : MonoBehaviour
{
    // Enemy Spawners
    [SerializeField] private SpawnerObstacles _obstacleSpawner;
    [SerializeField] private SpawnerFollowEnemy _followEnemySpawner;
    [SerializeField] private SpawnerPassEnemy _passEnemySpawner;

    // Phases
    [SerializeField] private float _phase1Duration;
    [SerializeField] private float _phase2Duration;
    private bool _isPhase1;
    private bool _isPhase2;

    // Obstacles
    private float _obsProbabilityP1 = 0.4f;
    private float _obsProbabilityP2 = 0.6f;
    private float _obsTime;

    // Follow Enemies
    private float _followEnemyDelayP1 = 0.2f;
    private float _followEnemyDelayP2 = 0.1f;
    private int _followEnemyAmountP1 = 3;
    private int _followEnemyAmountP2 = 4;
    private float _followEnemyTime;

    // Pass Enemies
    private float _passEnemyTime;

    private void Start()
    {
        StartCoroutine(LevelFlow());
    }

    private IEnumerator LevelFlow()
    {
        _isPhase1 = true;
        // 40% chance of spawning obstacles
        StartCoroutine(SpawnObstacles(_obsProbabilityP1));
        // 3 follow enemies with 0.2s delay between each spawn
        StartCoroutine(SpawnFollowEnemies(_followEnemyDelayP1, _followEnemyAmountP1));
        // 1 pass enemy every random 2-5 seconds
        StartCoroutine(SpawnPassEnemies(_passEnemyTime));
        // Wait for phase 1 to end
        yield return new WaitForSeconds(_phase1Duration);

        // Stop all coroutines from phase 1
        StopCoroutine(SpawnObstacles(_obsProbabilityP1));
        StopCoroutine(SpawnFollowEnemies(_followEnemyDelayP1, _followEnemyAmountP1));
        StopCoroutine(SpawnPassEnemies(_passEnemyTime));

        Debug.Log("Phase 1 ended");
    }

    private IEnumerator SpawnObstacles(float probability)
    {
        while (_isPhase1 || _isPhase2)
        {
            // Repeat the process every 5-7.5 seconds
            _obsTime = Random.Range(2.5f, 5.5f);
            _obstacleSpawner.TrySpawn(probability);
            yield return new WaitForSeconds(_obsTime);
        }
    }

    private IEnumerator SpawnFollowEnemies(float delay, int amount)
    {
        while (_isPhase1 || _isPhase2)
        {
            _followEnemyTime = Random.Range(6.5f, 8.5f);
            _followEnemySpawner.SpawnWave(delay, amount);
            yield return new WaitForSeconds(_followEnemyTime);
        }
    }

    private IEnumerator SpawnPassEnemies(float time)
    {
        while (_isPhase1 || _isPhase2)
        {
            _passEnemyTime = Random.Range(2f, 5f);
            _passEnemySpawner.Spawn();
            yield return new WaitForSeconds(_passEnemyTime);
        }
    }
}
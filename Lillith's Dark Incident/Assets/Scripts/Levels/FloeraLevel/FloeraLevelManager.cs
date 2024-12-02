using System.Collections;
using UnityEngine;

public class FloeraLevelManager : MonoBehaviour
{
    // Stage Settings
    private float _stageDuration = 20f;
    private float _midStage;
    private int _stageIndex = 1;
    private bool _isStageActive = true;

    // Enemies Values
    private float _obstacleProbabilityP1 = 0.50f;
    private float _obstacleProbabilityP2 = 0.65f;
    private float _obstacleCooldownP1 = 1.0f;
    private float _obstacleCooldownP2 = 1.0f;

    private float _followEnemyCooldownP1 = 7.5f;
    private int _followEnemyAmountP1 = 3;
    private float _followEnemyCooldownP2 = 7.0f;
    private int _followEnemyAmountP2 = 4;

    // Spawners
    [SerializeField] private SpawnerObstacles _spawnerObstacles;
    [SerializeField] private SpawnerFollowEnemy _spawnerFollowEnemy;
    [SerializeField] private SpawnerPassEnemy _spawnerPassEnemy;

    // Scroll Controller
    [SerializeField] private BackgroundOffsetController[] _backgroundController;

    // Control variables for coroutines
    private bool _isSpawningObstacles = false;
    private bool _isSpawningFollowEnemies = false;
    private bool _isSpawningPassEnemies = false;

    private void Start()
    {
        _midStage = _stageDuration / 2;
        StartCoroutine(PhaseOne());
    }

    private void Update()
    {
        if (_isStageActive && LocalTime.TimeScale != 0)
        {
            _stageDuration -= Time.deltaTime;
        }
    }

    private IEnumerator PhaseOne()
    {
        Debug.Log("Fase 1");

        if (!_isSpawningObstacles)
        {
            _isSpawningObstacles = true;
            StartCoroutine(SpawnObstacles(_obstacleProbabilityP1, _obstacleCooldownP1));
        }

        if (!_isSpawningFollowEnemies)
        {
            _isSpawningFollowEnemies = true;
            StartCoroutine(SpawnFollowEnemies(_followEnemyCooldownP1, _followEnemyAmountP1));
        }

        if (!_isSpawningPassEnemies)
        {
            _isSpawningPassEnemies = true;
            StartCoroutine(SpawnPassEnemies());
        }

        while (_stageIndex == 1)
        {
            if (_stageDuration <= _midStage)
            {
                _stageIndex = 2;
                StartCoroutine(PhaseTwo());
                StopCoroutine(PhaseOne());
            }
            yield return null;
        }
    }

    private IEnumerator PhaseTwo()
    {
        Debug.Log("Fase 2");
        for (int i = 0; i < _backgroundController.Length; i++)
        {
            _backgroundController[i].ApplyMultiplier();
        }

        GameEvents.TriggerPhaseTwoStart();

        if (!_isSpawningObstacles)
        {
            _isSpawningObstacles = true;
            StartCoroutine(SpawnObstacles(_obstacleProbabilityP2, _obstacleCooldownP2));
        }

        if (!_isSpawningFollowEnemies)
        {
            _isSpawningFollowEnemies = true;
            StartCoroutine(SpawnFollowEnemies(_followEnemyCooldownP2, _followEnemyAmountP2));
        }

        if (!_isSpawningPassEnemies)
        {
            _isSpawningPassEnemies = true;
            StartCoroutine(SpawnPassEnemies());
        }

        while (_stageIndex == 2)
        {
            if (_stageDuration <= 0)
            {
                _stageIndex = 3;
                _isStageActive = false;
                Debug.Log("Fase 2 completada");
                StartCoroutine(BossPreparatives());
                StopCoroutine(PhaseTwo());
            }
            yield return null;
        }
    }

    private IEnumerator BossPreparatives()
    {
        for (int i = 0; i < _backgroundController.Length; i++)
        {
            _backgroundController[i].StopScrolling();
        }
        yield return null;
    }

    private IEnumerator SpawnObstacles(float probability, float cooldown)
    {
        while (_isStageActive)
        {
            _spawnerObstacles.TrySpawn(probability);
            yield return new WaitForSeconds(cooldown);
        }
    }

    private IEnumerator SpawnFollowEnemies(float cooldown, int amount)
    {
        while (_isStageActive)
        {
            _spawnerFollowEnemy.SpawnWave(0.5f, amount);
            yield return new WaitForSeconds(cooldown);
        }
    }

    private IEnumerator SpawnPassEnemies()
    {
        while (_isStageActive)
        {
            var randomTime = Random.Range(2.5f, 6.5f);
            _spawnerPassEnemy.Spawn();
            yield return new WaitForSeconds(randomTime);
        }
    }
}

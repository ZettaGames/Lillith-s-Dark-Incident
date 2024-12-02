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
    private float _obstacleProbabilityP1 = 0.75f;
    private float _obstacleProbabilityP2 = 0.90f;
    private float _obstacleCooldownP1 = 0.5f;
    private float _obstacleCooldownP2 = 0.25f;

    private float _followEnemyCooldownP1 = 7.5f;
    private int _followEnemyAmountP1 = 3;
    private float _followEnemyCooldownP2 = 7.0f;
    private int _followEnemyAmountP2 = 4;

    // Spawners
    [SerializeField] private SpawnerObstacles _spawnerObstacles;
    [SerializeField] private SpawnerFollowEnemy _spawnerFollowEnemy;

    // Scroll Controller
    [SerializeField] private BackgroundOffsetController[] _backgroundController;

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

        while (_stageIndex == 1)
        {
            // 75% chance of spawning an obstacle every 0.5 seconds
            _spawnerObstacles.TrySpawn(_obstacleProbabilityP1);
            yield return new WaitForSeconds(_obstacleCooldownP1);

            // 3 follow enemies every 7.5 seconds
            _spawnerFollowEnemy.SpawnWave(0.5f, _followEnemyAmountP1);
            yield return new WaitForSeconds(_followEnemyCooldownP1);


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

        while (_stageIndex == 2)
        {
            // 90% chance of spawning an obstacle every 0.25 seconds
            _spawnerObstacles.TrySpawn(_obstacleProbabilityP2);
            yield return new WaitForSeconds(_obstacleCooldownP2);

            // 4 follow enemies every 7.0 seconds
            _spawnerFollowEnemy.SpawnWave(0.5f, _followEnemyAmountP2);
            yield return new WaitForSeconds(_followEnemyCooldownP2);

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
}
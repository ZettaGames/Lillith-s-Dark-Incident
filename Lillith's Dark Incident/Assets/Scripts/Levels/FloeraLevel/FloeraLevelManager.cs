using System.Collections;
using UnityEngine;

public class FloeraLevelManager : MonoBehaviour
{
	// Obstacles Settings
	private const float _obstacleSpawnProbability = 0.5f;
	private const float _obstacleSpawnDelay = 3.5f;
	
	// Spawners
	[Header("Spawners")]
	[SerializeField] private SpawnerObstacles _spawnerObstacles;
	
	private void Start()
	{
		StartCoroutine(PhaseOne());
	}
	
	private IEnumerator PhaseOne()
	{
		// 50% chance to spawn an obstacle every 3.5 seconds
		yield return SpawnObstacles();
	}
	
	private IEnumerator SpawnObstacles()
	{
		while (true)
		{
			_spawnerObstacles.TrySpawn(_obstacleSpawnProbability);
			yield return new WaitForSeconds(_obstacleSpawnDelay);
		}
	}
}
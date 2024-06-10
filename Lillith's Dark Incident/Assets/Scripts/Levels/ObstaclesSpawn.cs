using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawn : MonoBehaviour
{
	[Header("Obstacle Prefab")]
	[SerializeField] private GameObject obstaclePrefab;
	private List<GameObject> _obstacles = new List<GameObject>();
	
	[Header("Spawn Points")]
	private BoxCollider2D _spawnArea;
	
	private void Awake()
	{
		for (int i = 0; i < 10; i++)
		{
			GameObject obstacle = Instantiate(obstaclePrefab);
			obstacle.SetActive(false);
			_obstacles.Add(obstacle);
			obstacle.transform.SetParent(transform);
		}
	}
	
	private void Start()
	{
		_spawnArea = GetComponent<BoxCollider2D>();
		StartCoroutine(SpawnObstacles());
	}
	
	private IEnumerator SpawnObstacles()
	{
		while (true)
		{
			float randomTime = Random.Range(1f, 1.5f);
			yield return new WaitForSeconds(randomTime);
			SpawnObstacle();
		}
	}
	
	private void SpawnObstacle()
	{
		for (int i = 0; i < _obstacles.Count; i++)
		{
			if (!_obstacles[i].activeInHierarchy)
			{
				_obstacles[i].SetActive(true);
				_obstacles[i].transform.position = new Vector2(Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x), _spawnArea.bounds.max.y);
				return;
			}
		}
	}
}

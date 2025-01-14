using UnityEngine;

public class SpawnerGeneric : MonoBehaviour
{
	// Object Specifications
	[Header("Prefab")]
	[SerializeField] private GameObject _prefab;

	// Place where the objects will be spawned
	[Header("Spawn Area")]
	[SerializeField] private BoxCollider2D _spawnArea;
	protected BoxCollider2D SpawnArea => _spawnArea;

	protected void SpawnObject(Vector2 position)
	{
        // Instantiate the object
        GameObject obj = Instantiate(_prefab, position, Quaternion.identity);
        obj.transform.SetParent(transform);
    }
}
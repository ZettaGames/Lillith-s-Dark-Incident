using System.Collections.Generic;
using UnityEngine;

public class SpawnerGeneric : MonoBehaviour
{
    // Object Specifications
    [Header("Prefab")]
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _poolSize;
    private List<GameObject> _pool = new List<GameObject>();

    // Place where the objects will be spawned
    [Header("Spawn Area")]
    [SerializeField] private BoxCollider2D _spawnArea;

    // Property to access the spawn area
    protected BoxCollider2D SpawnArea => _spawnArea;

    private void Awake()
    {
        // Initialize the pool with the specified size
        for (int i = 0; i < _poolSize; i++)
        {
            var obj = Instantiate(_prefab);
            obj.SetActive(false);
            _pool.Add(obj);
            obj.transform.SetParent(transform);
        }
    }

    protected void SpawnObject(Vector2 position)
    {
        if (LocalTime.TimeScale == 0) return;

        // Get the first object in the pool that is not active
        var obj = _pool.Find(o => !o.activeInHierarchy);
        if (obj != null)
        {
            obj.transform.position = position;
            obj.SetActive(true);
        }
    }
}
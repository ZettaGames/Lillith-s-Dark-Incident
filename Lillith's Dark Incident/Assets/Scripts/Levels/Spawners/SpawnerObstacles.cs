using UnityEngine;

public class SpawnerObstacles : SpawnerGeneric
{
    public void TrySpawn(float probability)
    {
        if (Random.value < probability)
        {
            var position = new Vector2(Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x), SpawnArea.bounds.max.y);
            SpawnObject(position);
        }
    }
}
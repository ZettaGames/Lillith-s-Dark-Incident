using UnityEngine;

public class SpawnerPassEnemy : SpawnerGeneric
{
    public void Spawn()
    {
        var position = new Vector2(Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x), SpawnArea.bounds.max.y);
        SpawnObject(position);
    }
}
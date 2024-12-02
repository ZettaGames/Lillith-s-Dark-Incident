using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFollowEnemy : SpawnerGeneric
{
    public void SpawnWave(float delay, int amount)
    {
        StartCoroutine(SpawnWaveCoroutine(delay, amount));
    }

    private IEnumerator SpawnWaveCoroutine(float delay, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var position = new Vector2(Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x), SpawnArea.bounds.max.y);
            SpawnObject(position);
            yield return new WaitForSeconds(delay);
        }
    }
}
using UnityEngine;

public class SpawnerObstacles : SpawnerGeneric
{
	public void TrySpawn(float probability)
	{
		// Do not spawn if the game is paused
		if (LocalTime.TimeScale == 0) return;
		
		// Spawn with the specified probability
		if (Random.value < probability)
		{
			// Randomize the position within the spawn area
			var position = new Vector2(Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x), SpawnArea.bounds.max.y);
			SpawnObject(position);
		}
	}
}
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
	private const string POOL_TAG = "ObsPool";

	[Header("Obstacle Settings")]
	[SerializeField] private float _speed;
	[SerializeField] private float _destroyTime;

	private void Update()
	{
		transform.Translate(Vector2.down * _speed * Time.deltaTime * LocalTime.TimeScale);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(POOL_TAG))
		{
			gameObject.SetActive(false);
		}
	}
}

using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    // Constant tag for the pool of enemies
    private const string POOL_TAG = "EnemyPool";

    // Obstacle speed
    private float _speed = 6.75f;
	private float _speedMultiplier = 1.4f;

    private void Update()
	{
        transform.Translate(Vector2.down * _speed * Time.deltaTime * LocalTime.TimeScale);
	}

    private void IncreaseSpeed()
    {
        _speed *= _speedMultiplier;
    }

    private void OnEnable()
    {
        GameEvents.OnPhaseTwoStart += IncreaseSpeed;
    }

    private void OnDisable()
    {
        GameEvents.OnPhaseTwoStart -= IncreaseSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(POOL_TAG))
		{
			gameObject.SetActive(false);
		}
	}
}
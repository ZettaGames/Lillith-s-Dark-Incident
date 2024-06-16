using UnityEngine;

public class LillithBullet : MonoBehaviour
{
	[Header("Bullet")]
	[SerializeField] private float _bulletSpeed;
	
	private void Update()
	{
		// Move the bullet up.
		transform.position += transform.up * _bulletSpeed * Time.deltaTime;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		// Deactive the bullet when it collides with the pool or an enemy.
		if (other.CompareTag("Pool") || other.CompareTag("Enemy"))
		{
			gameObject.SetActive(false);
		}
	}
}
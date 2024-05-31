using UnityEngine;

public class LillithBullet : MonoBehaviour
{
	[Header("Bullet")]
	[SerializeField] private float _bulletSpeed;
	
	private void Update()
	{
		transform.position += transform.up * _bulletSpeed * Time.deltaTime;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Pool") || other.CompareTag("Enemy"))
		{
			gameObject.SetActive(false);
		}
	}
}
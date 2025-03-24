using System.Collections.Generic;
using UnityEngine;

public class LillithPoolManager : MonoBehaviour
{
	[Header("Bullet Pool settings")]
	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private int _bulletPoolSize;
	private List<GameObject> _bulletPool = new List<GameObject>();

	// Singleton instance
	private static LillithPoolManager _instance;
	public static LillithPoolManager Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}
	
	private void Start()
	{
		// Initialize the bullet pool with the desired amount of bullets.
		NewBullet(_bulletPoolSize);
	}

	private void NewBullet(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject bulletObject = Instantiate(_bulletPrefab);
			bulletObject.SetActive(false);
			_bulletPool.Add(bulletObject);
			bulletObject.transform.SetParent(transform);
		}
	}

	public GameObject ShootBullet()
	{
		for (int i = 0; i < _bulletPool.Count; i++)
		{
			if (!_bulletPool[i].activeInHierarchy)
			{
				_bulletPool[i].SetActive(true);
				return _bulletPool[i];
			}
		}
		NewBullet(1);
		_bulletPool[_bulletPool.Count - 1].SetActive(true);
		return _bulletPool[_bulletPool.Count - 1];
	}
}
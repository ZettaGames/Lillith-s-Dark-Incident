using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
	[Header("Obstacle Settings")]
	[SerializeField] private float _speed;
	[SerializeField] private float _destroyTime;

	private void OnEnable()
	{
		StartCoroutine(DestroyObstacle());
	}

	private void Update()
	{
		transform.Translate(Vector2.down * _speed * Time.deltaTime);
	}

	private IEnumerator DestroyObstacle()
	{
		yield return new WaitForSeconds(_destroyTime);
		gameObject.SetActive(false);
	}
}

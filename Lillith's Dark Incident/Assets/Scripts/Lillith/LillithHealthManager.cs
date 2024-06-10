using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LillithHealthManager : MonoBehaviour
{
	// ! Health variables
	[Header("Health")]
	[SerializeField] private int _currentStars;
	[SerializeField] private int _maxStars;
	[SerializeField] private Image[] _stars;

	// ! Sprites variables
	[Header("Sprites")]
	[SerializeField] private Sprite _fullStar;
	[SerializeField] private Sprite _emptyStar;

	// ! Damaging variables
	[Header("Damaging")]
	[SerializeField] private float _noControlTime;
	[SerializeField] private float _invincibilityTime;

	// ! Components variables
	private LillithController _lillithController;
	
	// ! Singleton
	private static LillithHealthManager _instance;
	public static LillithHealthManager Instance { get { return _instance; } }

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
		_lillithController = GetComponent<LillithController>();
	}

	private void Update()
	{
		if (_currentStars > _maxStars)
		{
			_currentStars = _maxStars;
		}

		for (int i = 0; i < _stars.Length; i++)
		{
			if (i < _currentStars)
			{
				_stars[i].sprite = _fullStar;
			}
			else
			{
				_stars[i].sprite = _emptyStar;
			}

			if (i < _maxStars)
			{
				_stars[i].enabled = true;
			}
			else
			{
				_stars[i].enabled = false;
			}
		}
	}

	#region health_functions
	public void TakeDamage(Vector2 position)
	{
		_currentStars--;
		StartCoroutine(NoControl());
		StartCoroutine(Invincibility());
		ScreenShake.Instance.Shake(0.1f, 5f);
		//_lillithController.KnockBack(position);
	}

	private IEnumerator NoControl()
	{
		_lillithController.CanMove = false;
		yield return new WaitForSeconds(_noControlTime);
		_lillithController.CanMove = true;
	}

	private IEnumerator Invincibility()
	{
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
		yield return new WaitForSeconds(_invincibilityTime);
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
	}
	#endregion
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			TakeDamage(transform.position);
		}
	}
}
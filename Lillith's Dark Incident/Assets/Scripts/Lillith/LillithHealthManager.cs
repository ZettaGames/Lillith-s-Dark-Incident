using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LillithController))]
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
	
	private void Start()
	{
		// Initialization of components
		_lillithController = GetComponent<LillithController>();
	}
	
	private void Update()
	{
		// Set the stars to the maximum amount
		if (_currentStars > _maxStars)
		{
			_currentStars = _maxStars;
		}
		
		for (int i = 0; i < _stars.Length; i++)
		{
			// Set the stars to full or empty
			if (i < _currentStars)
			{
				_stars[i].sprite = _fullStar;
			}
			else
			{
				_stars[i].sprite = _emptyStar;
			}
			
			// Enable or disable the stars
			if (i < _maxStars)
			{
				_stars[i].enabled = true;
			}
			else
			{
				_stars[i].enabled = false;
			}
		}
		
		// Kill the player if there are no stars left
		if (_currentStars <= 0)
		{
			Destroy(gameObject);
		}
	}
	
	#region damaging_methods
	public void TakeDamage()
	{
		// Decrease the amount of stars
		_currentStars--;
		
		// Apply the damaging effects
		StartCoroutine(NoControl());
		StartCoroutine(Invincibility());
	}
	
	private IEnumerator NoControl()
	{
		// Disable the player's movement
		_lillithController.CanMove = false;
		
		// Wait for a certain amount of time
		yield return new WaitForSeconds(_noControlTime);
		
		// Enable the player's movement
		_lillithController.CanMove = true;
	}
	
	private IEnumerator Invincibility()
	{
		// Transparent the player
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
		
		// Wait for a certain amount of time
		yield return new WaitForSeconds(_invincibilityTime);
		
		// Make the player visible
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
	}
	#endregion
}
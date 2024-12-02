using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(LillithController))]
public class LillithHealthManager : MonoBehaviour
{
	// ! Constants for the tags
	private const string PLAYER_TAG = "Player";
	private const string UNTAGGED = "Untagged";
	
	// ! Constants for the shake
	private const float SHAKE_INTENSITY = 0.1f;
	private const float GAMEPAD_SHAKE = 0.5f;
	private const float SHAKE_DURATION = 0.25f;
	
	// ! Colors for invulnerability
	private readonly Color _traslucidColor = new Color(1, 1, 1, 0.75f);
	private readonly Color _solidColor = new Color(1, 1, 1, 1);
	
	// ! Health variables
	[Header("Health")]
	[SerializeField] private int currentStars;
	public int CurrentStars { get { return currentStars; } }
	[SerializeField] private int _maxStars;
	[SerializeField] private Image[] _stars;
	private bool hasDied = false;

    // ! Sprites variables
    [Header("Sprites")]
	[SerializeField] private Sprite _fullStar;
	[SerializeField] private Sprite _emptyStar;

	// ! Damaging variables
	[Header("Damaging")]
	[SerializeField] private float _noControlTime;
	[SerializeField] private float _invincibilityTime;

	// ! Components variables
	private Animator _animator;
	private LillithController _lillithController;

	private void Start()
	{
		// Initialization of components
        _animator = GetComponent<Animator>();
		_lillithController = GetComponent<LillithController>();
    }

	private void Update()
	{
        // Kill the player if there are no stars left
        if (currentStars <= 0)
        {
            if (!hasDied)
            {
                hasDied = true;
                StartCoroutine(Death());
            }
        }

        // Set the stars to the maximum amount
        if (currentStars > _maxStars)
		{
			currentStars = _maxStars;
		}

		for (int i = 0; i < _stars.Length; i++)
		{
			// Set the stars to full or empty
			if (i < currentStars)
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
	}

	#region damaging_methods
	public void TakeDamage()
	{
		// Decrease the amount of stars
		currentStars--;

        // Apply the damaging effects
        StartCoroutine(NoControl());
		StartCoroutine(Invincibility());
		
		// Shake the screen
		ScreenShake.Instance.Shake(SHAKE_INTENSITY, SHAKE_DURATION);
		
		// Shake the gamepad
		StartCoroutine(ShakeGamepad(GAMEPAD_SHAKE, GAMEPAD_SHAKE, SHAKE_DURATION));
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
        // Make the player traslucid and untargetable
        GetComponent<SpriteRenderer>().color = _traslucidColor;
		gameObject.tag = UNTAGGED;
		gameObject.layer = LayerMask.NameToLayer("Default");

		float elapsedTime = 0.0f; // Elapsed time since the invincibility started

		// Loop until the invincibility time is over
		while (elapsedTime < _invincibilityTime)
		{
			// Only count the time if the game is not paused
			if (LocalTime.TimeScale != 0.0f)
			{
				elapsedTime += Time.deltaTime;
			}
			yield return null;
		}

		// Make the player solid and targetable
		GetComponent<SpriteRenderer>().color = _solidColor;
		gameObject.tag = PLAYER_TAG;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
	
	private IEnumerator ShakeGamepad(float lowFreqIntensity, float highFreqIntensity, float duration)
	{
		// Check if there is a gamepad connected
		if (Gamepad.current != null)
		{
			// Shake the gamepad for a certain amount of time
			Gamepad.current.SetMotorSpeeds(lowFreqIntensity, highFreqIntensity);
			yield return new WaitForSeconds(duration);
			Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
		}
	}
	#endregion

	private IEnumerator Death()
	{
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);

        Debug.Log("Before pausing the game");
        LocalTime.TimeScale = 0.0f;

        Debug.Log("Before deactivating the collider");
        GetComponent<CircleCollider2D>().enabled = false;

		Debug.Log("Before waiting for 2 seconds");
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Before playing the death animation");
        _animator.SetTrigger("death");

        Debug.Log("Before waiting for 3.5 seconds");
        yield return new WaitForSeconds(2.5f);

        Debug.Log("Before loading the level");
        LevelLoader.Instance.LoadLevel(2);

		Debug.Log("Before waiting for 1 second");
        yield return new WaitForSeconds(1.2f);

        Debug.Log("Before setting the time scale to 1");
        LocalTime.TimeScale = 1.0f;
    }
}
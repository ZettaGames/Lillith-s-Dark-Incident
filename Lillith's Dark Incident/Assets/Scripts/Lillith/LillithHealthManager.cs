using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(LillithController))]
public class LillithHealthManager : MonoBehaviour
{
    // Constant of the menu scene
    private const int MAIN_MENU = 2;

    // Constant for the animator
    private const string DEATH = "death";

    // Constants for the tags and layers
    private const string PLAYER = "Player";
	private const string ENEMY = "Enemy";
	private const string OBSTACLE = "Obstacle";
	private const string UNTAGGED = "Untagged";
	private const string DEFAULT = "Default";

    // Constants for the shake
    private const float SCREEN_SHAKE = 0.1f;
	private const float GAMEPAD_SHAKE = 0.5f;
	private const float SHAKE_DURATION = 0.25f;

	// Colors for invulnerability
	private readonly Color _traslucid = new Color(1, 1, 1, 0.75f);
	private readonly Color _solid = new Color(1, 1, 1, 1);

	// Health variables
	[Header("Health")]
	[SerializeField] private int currentStars;
	[SerializeField] private int _maxStars;
	[SerializeField] private Image[] _stars;
	public int CurrentStars { get { return currentStars; } }
	private bool hasDied = false;

	// Sprites variables
	[Header("Sprites")]
	[SerializeField] private Sprite _fullStar;
	[SerializeField] private Sprite _emptyStar;

	// Damaging variables
	[Header("Damaging")]
	[SerializeField] private float _noControlTime;
	[SerializeField] private float _invincibilityTime;
	
	[Header("Knockback")]
	[SerializeField] private Vector2 _knockBackForce;

	// Components variables
	private Animator _animator;
	private Rigidbody2D _rigidbody;
	private LillithController _lillithController;

	private void Start()
	{
		// Initialization of components
		_animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_lillithController = GetComponent<LillithController>();
		
		// Set the stars to the maximum amount
		if (currentStars > _maxStars)
		{
			currentStars = _maxStars;
		}
	}

	private void Update()
	{	
		// Kill the player if there are no stars left
		if (currentStars <= 0 && !hasDied)
		{
			hasDied = true;
			StartCoroutine(Death());
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
		ScreenShake.Instance.Shake(SCREEN_SHAKE, SHAKE_DURATION);
		// Shake the gamepad
		_lillithController.ShakeGamepad(GAMEPAD_SHAKE, GAMEPAD_SHAKE, SHAKE_DURATION);
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
		GetComponent<SpriteRenderer>().color = _traslucid;
		gameObject.tag = UNTAGGED;
		gameObject.layer = LayerMask.NameToLayer(DEFAULT);

        // Elapsed time since the invincibility started
        float elapsedTime = 0.0f;

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
		GetComponent<SpriteRenderer>().color = _solid;
		gameObject.tag = PLAYER;
		gameObject.layer = LayerMask.NameToLayer(PLAYER);
	}
	
	private void KnockBack(Vector2 hitPoint)
	{
		// Move the player from the hit point
		Vector2 direction = (Vector2.zero - hitPoint).normalized;
		_rigidbody.velocity = direction * _knockBackForce;
	}

	private IEnumerator Death()
	{
        // Make the player solid
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);

        // Stop the game
        LocalTime.TimeScale = 0.0f;

        // Prevent more collisions
        GetComponent<CircleCollider2D>().enabled = false;
		yield return new WaitForSeconds(0.25f);

        // Play the death animation
        _animator.SetTrigger(DEATH);
		yield return new WaitForSeconds(1.5f);

        // Load the main menu and resume the game
        LevelLoader.Instance.LoadLevel(MAIN_MENU);
		yield return new WaitForSeconds(1.2f);
		LocalTime.TimeScale = 1.0f;
	}
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with an enemy
        if ((collision.collider.CompareTag(ENEMY) || collision.collider.CompareTag(OBSTACLE)) && gameObject.CompareTag(PLAYER))
        {
            TakeDamage();
            KnockBack(collision.collider.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with an enemy
        if ((other.CompareTag(ENEMY) || other.CompareTag(OBSTACLE)) && gameObject.CompareTag(PLAYER))
        {
            TakeDamage();
            KnockBack(other.transform.position);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage();
        KnockBack(other.transform.position);
    }
}
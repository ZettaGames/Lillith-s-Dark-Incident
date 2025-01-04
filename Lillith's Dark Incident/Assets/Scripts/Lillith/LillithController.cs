using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(Animator))]
public class LillithController : MonoBehaviour
{
	#region variables
	// Constant for the animator
	private const string SPEED = "xSpeed";
	private const string BREAK = "Break";

    // Constants for the input actions
    private const string MOVE = "Move";
	private const string SHOOT = "Shoot";
	private const string SUPER = "Super";
	private const string SHIELD = "Shield";

    [Header("Movement")]
	// Movement variables
	[SerializeField] private float _moveSpeed;
	[SerializeField, Range(0.05f, 0.15f)] private float _smoothMove;
	public bool CanMove { get; set; } = true;

	// Vector variables
	private Vector2 _input;
	private Vector2 _fixedInput;
	private Vector2 _velocity = Vector2.zero;

	[Header("Shooting")]
	[SerializeField, Range(0.01f, 0.05f)] private float _shootCooldown;
	private float _shootTimer;

	[Header("Super Attack")]
	[SerializeField] private GameObject _superAttack;
    [SerializeField] private float _superCooldown;
    private float _superTimer;

	[Header("Super UI")]
    [SerializeField] private TMP_Text _superText;
	[SerializeField] private Image _superIcon;

    [Header("Shield")]
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _shieldDuration;
    [SerializeField] private float _shieldCooldown;
    private float _shieldTimer;

	[Header("Shield UI")]
    [SerializeField] private TMP_Text _shieldText;
    [SerializeField] private Image _shieldIcon;

    // Unity components
    private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;
	private Animator _animator;
	#endregion

	#region unity_methods
	private void Start()
	{
		// Initialization of components
		_rigidbody = GetComponent<Rigidbody2D>();
		_playerInput = GetComponent<PlayerInput>();
		_animator = GetComponent<Animator>();

        // Hability cooldowns
        _shootTimer = _shootCooldown;
        _superTimer = _superCooldown;
        _shieldTimer = _shieldCooldown;
    }

    private void Update()
	{
		// Update the animator
		_animator.SetFloat(SPEED, _input.x);

        // Update the UI for Super Attack
        if (_superTimer >= _superCooldown)
        {
            _superText.text = ": Ready";
            _superIcon.color = Color.white;
        }
        else
        {
            _superText.text = $": {(int)(_superCooldown - _superTimer)} s";
            _superIcon.color = Color.grey;
        }

        // Update the UI for Shield
        if (_shieldTimer >= _shieldCooldown)
        {
            _shieldText.text = ": Ready";
            _shieldIcon.color = Color.white;
        }
        else
        {
            _shieldText.text = $": {(int)(_shieldCooldown - _shieldTimer)} s";
            _shieldIcon.color = Color.grey;
        }

        if (LocalTime.TimeScale > 0.0f)
		{
			// Shoot
			_shootTimer += Time.deltaTime;
			Shoot();

			// Read input
			ReadInput();

            // Super shoot
            _superTimer += Time.deltaTime;
            SuperShoot();

            // Shield
            _shieldTimer += Time.deltaTime;
            BubbleShield();
        }
    }

	private void FixedUpdate()
	{
		// Move the player
		if (CanMove)
		{
			Move();
		}
	}
	#endregion

	private void ReadInput()
	{
		// Read input from the player
		_input = _playerInput.actions[MOVE].ReadValue<Vector2>().normalized;
	}

    #region lillith_actions
    private void Move()
	{
		// Smooth the input
		_fixedInput = Vector2.SmoothDamp(_fixedInput, _input, ref _velocity, _smoothMove);

		// Move the player
		_rigidbody.velocity = _fixedInput * _moveSpeed * LocalTime.TimeScale;
	}

	private void Shoot()
	{
		if (_playerInput.actions[SHOOT].IsPressed() && _shootTimer >= _shootCooldown)
		{
			// Reset the timer
			_shootTimer = 0f;

			// Shoot a bullet from the pool manager
			var bullet = LillithPoolManager.Instance.ShootBullet();
			bullet.transform.position = transform.position;
			bullet.transform.rotation = transform.rotation;
		}
	}

	private void SuperShoot()
	{
		if (_playerInput.actions[SUPER].IsPressed() && _superTimer >= _superCooldown)
		{
            // Reset the timer
            _superTimer = 0f;

            // Launch the player backwards and shoot the super attack
            StartCoroutine(Super());
        }
    }

    private void BubbleShield()
    {
        if (_playerInput.actions[SHIELD].IsPressed() && _shieldTimer >= _shieldCooldown)
        {
            // Reset the timer
            _shieldTimer = 0f;

            // Create the shield
            StartCoroutine(Bubble());
        }
    }
    #endregion

	private IEnumerator Super()
	{
		CanMove = false;
		_rigidbody.velocity = Vector2.down * 2.5f;
		ShakeGamepad(0.25f, 0.25f, 0.25f);
        Instantiate(_superAttack, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        CanMove = true;
    }

	private IEnumerator Bubble()
	{
        CanMove = false;
        _rigidbody.velocity = Vector2.zero;
        var bubble = Instantiate(_shield, transform.position, Quaternion.identity);
        bubble.transform.parent = transform;
        yield return new WaitForSeconds(0.3f);
        CanMove = true;
        GetComponentInChildren<LillithShield>().Break(_shieldDuration, BREAK);
    }

    #region gamepad_shake
    public void ShakeGamepad(float lowFreqIntensity, float highFreqIntensity, float duration)
	{
		StartCoroutine(GamepadShake(lowFreqIntensity, highFreqIntensity, duration));
	}
	
	private IEnumerator GamepadShake(float lowFreqIntensity, float highFreqIntensity, float duration)
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
}
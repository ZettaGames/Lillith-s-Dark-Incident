using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput), typeof(Animator))]
public class LillithController : MonoBehaviour
{
	#region variables
	// ! Constant for the animator
	private const string SPEED_TRIGGER = "xSpeed";
	
	// ! Constants for the input actions
	private const string MOVE_ACTION = "Move";
	private const string SHOOT_ACTION = "Shoot";
	
	// ! Movement-related settings
	[Header("Movement")]
	// ? Movement variables
	[SerializeField] private float _moveSpeed;
	[SerializeField, Range(0.05f, 0.15f)] private float _smoothMove;
	
	// ? Vector variables
	private Vector2 _input;
	private Vector2 _fixedInput;
	private Vector2 _velocity = Vector2.zero;
	
	// ! Shooting-related settings
	[Header("Shooting")]
	[SerializeField, Range(0.01f, 0.05f)] private float _shootCooldown;
	private float _shootTimer;
	
	// ! Damage-related settings
	[Header("Damage")]
	[SerializeField] private Vector2 _knockBackForce;
	public bool CanMove { get; set; } = true;
	
	// ! Components
	// ? Unity components
	private Rigidbody2D _rigidbody;
	private PlayerInput _playerInput;
	private Animator _animator;
	
	// ? Script components
	private LillithHealthManager _lillithHealthManager;
	#endregion
		
	#region unity_methods
	private void Start()
	{
		// Initialization of components
		_rigidbody = GetComponent<Rigidbody2D>();
		_playerInput = GetComponent<PlayerInput>();
		_animator = GetComponent<Animator>();
		_lillithHealthManager = GetComponent<LillithHealthManager>();
	}
	
	private void Update()
	{
		// Update the animator
		_animator.SetFloat(SPEED_TRIGGER, _input.x);
		
		// Shoot
		_shootTimer += Time.deltaTime;
		Shoot();
		
		// Read input
		ReadInput();
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
	
	#region lillith_actions
	private void ReadInput()
	{
		// Read input from the player
		_input = _playerInput.actions[MOVE_ACTION].ReadValue<Vector2>().normalized;
	}
	
	private void Move()
	{
		// Smooth the input
		_fixedInput = Vector2.SmoothDamp(_fixedInput, _input, ref _velocity, _smoothMove);
		
		// Move the player
		_rigidbody.velocity = _fixedInput * _moveSpeed;
	}
	
	private void Shoot()
	{
		if (_playerInput.actions[SHOOT_ACTION].IsPressed() && _shootTimer >= _shootCooldown)
		{
			// Reset the timer
			_shootTimer = 0f;
			
			// Shoot a bullet from the pool manager
			var bullet = LillithPoolManager.Instance.ShootBullet();
			bullet.transform.position = transform.position;
			bullet.transform.rotation = transform.rotation;
		}
	}
	#endregion
	
	#region damaging_methods
	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check if the player collided with an enemy
		if (other.CompareTag("Enemy"))
		{
			// Take damage
			KnockBack(transform.position);
			_lillithHealthManager.TakeDamage();
		}
	}
	
	private void KnockBack(Vector2 hitPoint)
	{
		Vector2 direction = (Vector2.zero - hitPoint).normalized;
		_rigidbody.velocity = direction * _knockBackForce;
	}
	
	private IEnumerator CancelInput()
	{
		_playerInput.enabled = false;
		yield return new WaitForSeconds(0.25f);
		_playerInput.enabled = true;
	}
	#endregion
}
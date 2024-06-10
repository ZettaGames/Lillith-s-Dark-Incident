using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Animator))]
public class LillithController : MonoBehaviour
{
	#region variables
	// ! Tags
	private const string BORDER_TAG = "Border";

	// ! Action Map actions names
	private const string MOVE = "Move";
	private const string SHOOT = "Shoot";

	// ! Movement-related variables
	[Header("Movement")]

	// ? Number variables
	[SerializeField] private float _moveSpeed;
	[SerializeField, Range(0, 0.1f)] private float _moveSmoothness;

	// ? Vector variables
	private Vector2 _input;
	private Vector2 _finalInput;
	private Vector2 _speed = Vector2.zero;
	
	// ? Move controller
	public bool CanMove { get; set; } = true;

	// ! Shooting-related variables
	[Header("Shooting")]
	[SerializeField, Range(0, 0.05f)] private float _bulletCooldown;
	private float _bulletCooldownTimer;

	// ! Collision variables
	private Dictionary<GameObject, Vector2> _borderNormals = new Dictionary<GameObject, Vector2>();

	// ! Components variables
	private PlayerInput _playerInput;
	private Animator _animator;
	#endregion

	#region unity_functions
	private void Awake()
	{
		// Components initialization
		_playerInput = GetComponent<PlayerInput>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		// 
		_bulletCooldownTimer += Time.deltaTime;

		// Handle right-left animation
		_animator.SetFloat("xSpeed", _input.x);

		// Actions
		HandleBorderCollisions();
		ReadInput();
		Shoot();

		if (CanMove)
		{
			Move();
		}
	}
	#endregion

	#region actions_functions
	private void ReadInput()
	{
		_input = _playerInput.actions[MOVE].ReadValue<Vector2>().normalized;
	}
	
	private void Move()
	{
		// Smooth movement with Vector2.SmoothDamp
		// For more information: https://docs.unity3d.com/ScriptReference/Vector2.SmoothDamp.html
		_finalInput = Vector2.SmoothDamp(_finalInput, _input, ref _speed, _moveSmoothness);
		transform.position += new Vector3(_finalInput.x, _finalInput.y, 0) * _moveSpeed * Time.deltaTime;
	}

	private void Shoot()
	{
		if (_playerInput.actions[SHOOT].IsPressed() && _bulletCooldownTimer > _bulletCooldown)
		{
			_bulletCooldownTimer = 0;
			// Instantiate from pool manager script
			GameObject bullet = LillithPoolManager.Instance.ShootBullet();
			bullet.transform.position = transform.position + transform.up * 0.5f;
			bullet.transform.rotation = transform.rotation;
		}
	}
	#endregion
	
	#region collision_functions
	private void HandleBorderCollisions()
	{
		foreach (Vector2 borderNormal in _borderNormals.Values)
		{
			if (Vector2.Dot(borderNormal, _input) < 0)
			{
				// Counteract the movement if hitting a border
				_input -= Vector2.Dot(_input, borderNormal) * borderNormal;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// Stop the player in the direction of the border
		if (other.gameObject.CompareTag(BORDER_TAG))
		{
			_borderNormals[other.gameObject] = other.contacts[0].normal;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag(BORDER_TAG))
		{
			_borderNormals.Remove(other.gameObject);
		}
	}
	#endregion
}
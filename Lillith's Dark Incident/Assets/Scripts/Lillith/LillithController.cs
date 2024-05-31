using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LillithController : MonoBehaviour
{
	#region variables
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

	// ! Shooting-related variables
	[Header("Shooting")]
	[SerializeField, Range(0, 0.05f)] private float _bulletCooldown;
	private float _bulletCooldownTimer;

	// ! Taking Damage-related variables
	[Header("Taking Damage")]
	[SerializeField] private Vector2 _knockbackForce;
	[HideInInspector] public bool canMove = true;

	// ! Collision variables
	private Dictionary<GameObject, Vector2> _borderNormals = new Dictionary<GameObject, Vector2>();

	// ! Components variables
	private PlayerInput _playerInput;
	private Animator _animator;
	#endregion

	private void Awake()
	{
		// Components initialization
		_playerInput = GetComponent<PlayerInput>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		_bulletCooldownTimer += Time.deltaTime;

		_animator.SetFloat("xSpeed", _input.x);

		ReadInput();
		Shoot();

		if (canMove)
		{
			Move();
		}
	}

	private void ReadInput()
	{
		_input = _playerInput.actions[MOVE].ReadValue<Vector2>().normalized;

		foreach (Vector2 borderNormal in _borderNormals.Values)
		{
			if (Vector2.Dot(borderNormal, _input) < 0)
			{
				_input -= Vector2.Dot(_input, borderNormal) * borderNormal;
			}
		}
	}

	private void Move()
	{
		_finalInput = Vector2.SmoothDamp(_finalInput, _input, ref _speed, _moveSmoothness);
		transform.position += new Vector3(_finalInput.x, _finalInput.y, 0) * _moveSpeed * Time.deltaTime;
	}

	private void Shoot()
	{
		if (_playerInput.actions[SHOOT].IsPressed() && _bulletCooldownTimer > _bulletCooldown)
		{
			_bulletCooldownTimer = 0;
			GameObject bullet = LillithPoolManager.Instance.ShootBullet();
			bullet.transform.position = transform.position + transform.up * 0.5f;
			bullet.transform.rotation = transform.rotation;
		}
	}

	public void KnockBack(Vector2 hitPoint)
	{
		Vector2 direction = (Vector2.zero - hitPoint).normalized;
		transform.position += new Vector3(direction.x, direction.y, 0) * _knockbackForce.magnitude;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Border"))
		{
			_borderNormals[other.gameObject] = other.contacts[0].normal;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Border"))
		{
			_borderNormals.Remove(other.gameObject);
		}
	}
}
using System.Collections;
using UnityEngine;

public class FollowEnemyBehaviour : MonoBehaviour
{
    // Constants
    private const string POOL_TAG = "EnemyPool";
    private const string BULLET_TAG = "Bullet";

    // Enemy settings
    private int _health = 25;
    private float _speed = 3f;
    private float _speedMultiplier = 1.4f;

    // Player follow settings
    private float _followDistance = 2.0f;
    private float _followTime = 20f;
    private float _followTimer;

    // The distance to maintain from other objects of the same type
    private float _minDistanceBetweenEnemies = 5f;

    // Player reference
    [Header("References")]
    private Transform _player;
    [SerializeField] private BulletSystem _bulletSystem;

    private void Awake()
    {
        _bulletSystem.Target = _player;
    }

    private void Start()
    {
        // Initialization of components
        _player = GameObject.FindObjectOfType<LillithController>().transform;

        // Initialize the bullet system
        SetBulletPatternP1();
        _bulletSystem.StartSpawner();

        // Initialize the follow timer
        _followTimer = _followTime;
    }

    private void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (_followTimer > 0)
        {
            // Follow the player while maintaining a certain distance
            if (distanceToPlayer > _followDistance)
            {
                Vector2 direction = (_player.position - transform.position).normalized;
                transform.position += (Vector3)direction * _speed * LocalTime.TimeScale * Time.deltaTime;
            }

            // Decrease the follow timer
            if (LocalTime.TimeScale > 0)
            {
                _followTimer -= Time.deltaTime;
            }
        }
        else
        {
            // Move downwards after follow time is over
            transform.position += Vector3.down * _speed * LocalTime.TimeScale * Time.deltaTime;
            _bulletSystem.StopSpawner();
        }
    }

    private void StartPhase2()
    {
        // Set the new pattern
        SetBulletPatternP2();

        // Increase the speed
        _speed *= _speedMultiplier;
    }

    private void SetBulletPatternP1()
    {
        // Set the follow pattern
        _bulletSystem.CurrentPattern = BulletSystem.PatternType.TargetShoot;
        _bulletSystem.Target = _player;

        // Shoot in a straight line
        _bulletSystem.CurrentEmitter = BulletSystem.EmitterShape.Straight;
        _bulletSystem.UseEmitter = false;

        // Shooting configuration
        _bulletSystem.FireRate = 0.75f;
        _bulletSystem.FollowRefreshRate = 0.05f;
    }

    private void SetBulletPatternP2()
    {
        // Stop the current pattern
        _bulletSystem.StopSpawner();

        // Set the follow pattern
        _bulletSystem.CurrentPattern = BulletSystem.PatternType.TargetShoot;
        _bulletSystem.Target = _player;

        // Shoot in a cone
        _bulletSystem.CurrentEmitter = BulletSystem.EmitterShape.Cone;
        _bulletSystem.UseEmitter = true;

        // Shooting configuration
        _bulletSystem.FireRate = 0.5f;
        _bulletSystem.FollowRefreshRate = 0.0f;

        // Start the new pattern
        _bulletSystem.StartSpawner();
    }

    private IEnumerator TakeDamage()
    {
        // Decrease the health
        _health--;

        // Make the enemy blink
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        if (_health <= 0)
        {
            // Stop the following
            _followTimer = 0;

            // Deactivate renderer and collider
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // Stop the bullet system
            _bulletSystem.StopSpawner();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(POOL_TAG))
        {
            gameObject.SetActive(false);
        }

        if (collision.CompareTag(BULLET_TAG))
        {
            StartCoroutine(TakeDamage());
        }
    }

    private void OnEnable()
    {
        // Reset the enemy
        _health = 25;
        _followTimer = _followTime;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        SetBulletPatternP1();
        _bulletSystem.Target = _player;
        _bulletSystem.StartSpawner();

        // Subscribe to the phase two event
        GameEvents.OnPhaseTwoStart += StartPhase2;
    }

    private void OnDisable()
    {
        GameEvents.OnPhaseTwoStart -= StartPhase2;
    }
}
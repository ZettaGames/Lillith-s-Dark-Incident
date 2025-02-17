using System.Collections;
using UnityEngine;

public class FollowEnemyBehaviour : MonoBehaviour
{
    // Constants
    private const string POOL_TAG = "EnemyPool";
    private const string BULLET_TAG = "Bullet";
    private const string NO_COLLISION = "NoCollision";
    private const string SUPER = "Super";
    private const string CHECK = "Check";

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] private int _health = 10;
    [SerializeField] private float _speed = 1.75f;

    // Follow Settings
    [Header("Follow Settings")]
    [SerializeField] private float _followTime = 5.5f;
    [SerializeField] private float _followRadius = 3.5f;
    private float _followTimer = 0.0f;
    private Transform _followTarget;

    // Avoidance Settings
    [Header("Avoidance Settings")]
    [SerializeField] private float _avoidanceRadius = 1.5f;

    // Bullet Spawner Component
    private BulletSystem _bulletSystem;

    private void Awake()
    {
        // Set the target from the player layer
        _followTarget = GameObject.FindObjectOfType<LillithController>().transform;
    }

    private void Start()
    {
        // Initialization of the bullet system
        _bulletSystem = GetComponent<BulletSystem>();
        _bulletSystem.Target = _followTarget;
        _bulletSystem.StartSpawner();
    }

    private void Update()
    {
        // Update the timer
        if (LocalTime.TimeScale > 0.0f)
        {
            _followTimer += Time.deltaTime;
        }

        if (_followTimer < _followTime && LocalTime.TimeScale > 0.0f)
        {
            FollowTarget();
        }
        else
        {
            // Stop the spawner and move the enemy down
            _bulletSystem.StopSpawner();
            transform.position += Vector3.down * _speed * Time.deltaTime * LocalTime.TimeScale;
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void FollowTarget()
    {
        // Stop following if the target is null
        if (_followTarget == null)
        {
            return;
        }

        // Calculating the direction and distance to the target
        Vector3 direction = _followTarget.position - transform.position;
        float distance = direction.magnitude;
        // Tolerance for the distance to avoid jittering
        float epsilon = 0.05f;

        // Move the enemy based on the distance to the target
        if (distance > _followRadius + epsilon)
        {
            // Move the enemy towards the target
            Vector3 moveDirection = direction.normalized * _speed * Time.deltaTime;
            transform.position += moveDirection;
        }
        else if (distance < _followRadius - epsilon)
        {
            // Move the enemy away from the target
            Vector3 moveDirection = -direction.normalized * _speed * Time.deltaTime;
            transform.position += moveDirection;
        }

        // Avoid other enemies
        AvoidOtherEnemies();
    }

    private IEnumerator TakeDamage()
    {
        // Reduce the health
        _health--;

        // Change the enemy color
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        // Check if the enemy is dead
        if (_health <= 0)
        {
            // Deactivate the enemy
            _bulletSystem.StopSpawner();
            gameObject.tag = NO_COLLISION;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }


    private void AvoidOtherEnemies()
    {
        // Find all enemies in the scene
        FollowEnemyBehaviour[] enemies = FindObjectsOfType<FollowEnemyBehaviour>();

        foreach (var enemy in enemies)
        {
            // Skip self
            if (enemy == this) continue;

            // Calculate the distance to the other enemy
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            // If the other enemy is within the avoidance radius, move away
            if (distanceToEnemy < _avoidanceRadius)
            {
                Vector3 moveDirection = -directionToEnemy.normalized * _speed * Time.deltaTime;
                transform.position += moveDirection;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a bullet
        if (collision.gameObject.CompareTag(BULLET_TAG))
        {
            // Update the score
            LevelScoreManager.Instance.EnemyHitBonus();

            // Take damage
            StartCoroutine(TakeDamage());
        }

        // Check if the collision is with the super attack
        if (collision.gameObject.CompareTag(SUPER))
        {
            _health = 1;
            StartCoroutine(TakeDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with a bullet
        if (collision.gameObject.CompareTag(BULLET_TAG))
        {
            // Update the score
            LevelScoreManager.Instance.EnemyHitBonus();

            // Take damage
            StartCoroutine(TakeDamage());
        }

        // Check if the collision is with the pool
        if (collision.gameObject.CompareTag(POOL_TAG))
        {
            // Stop the spawner
            _bulletSystem.InstantKill();

            // Destroy the enemy
            Destroy(gameObject);
        }

        // Check if the collision is with the super attack
        if (collision.CompareTag(SUPER))
        {
            _health = 1;
            StartCoroutine(TakeDamage());
        }

        // Deactive trigger collision if collides with check
        if (collision.CompareTag(CHECK))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
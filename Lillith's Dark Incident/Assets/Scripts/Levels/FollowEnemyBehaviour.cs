using System.Collections;
using UnityEngine;

public class FollowEnemyBehaviour : MonoBehaviour
{
    // Constants
    private const string POOL_TAG = "EnemyPool";
    private const string BULLET_TAG = "Bullet";
    private const string NO_COLLISION = "NoCollision";
    private const string SUPER = "Super";

    // Enemy Settings
    private int _health = 10;
    private float _speed = 1.75f;

    // Follow Settings
    private float _followTime = 5.5f;
    private float _followRadius = 3.5f;
    private float _followTimer = 0.0f;
    private Transform _followTarget;

    // Bullet Spawner Component
    private BulletSystem _bulletSystem;

    private void Start()
    {
        // Initialization of the bullet system
        _bulletSystem = GetComponent<BulletSystem>();
        _followTarget = _bulletSystem.Target;
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
        float epsilon = 0.01f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a bullet
        if (collision.gameObject.CompareTag(BULLET_TAG))
        {
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
    }
}
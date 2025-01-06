using System.Collections;
using UnityEngine;

public class PassEnemyBehaviour : MonoBehaviour
{
    // Constants
    private const string POOL_TAG = "EnemyPool";
    private const string BULLET_TAG = "Bullet";
    private const string FINISH = "Finish";
    private const string NO_COLLISION = "NoCollision";
    private const string SUPER = "Super";

    // Enemy Settings
    private int _health = 15;
    private float _speed = 1.25f;
    private float _moveTime = 3.0f;
    private float _stopTime = 1.5f;
    private float _stopChance = 0.5f;
    private bool _isMoving = true;

    // Bullet Spawner Component
    private BulletSystem _bulletSystem;

    private void Start()
    {
        // Initialization of the bullet system
        _bulletSystem = GetComponent<BulletSystem>();
        _bulletSystem.StartSpawner();
        StartCoroutine(MoveAndStop());
    }

    private void Update()
    {
        if (_isMoving)
        {
            transform.position += Vector3.down * _speed * Time.deltaTime * LocalTime.TimeScale;
        }
    }

    private IEnumerator MoveAndStop()
    {
        while (true)
        {
            if (Random.value < _stopChance)
            {
                _isMoving = false;
                yield return new WaitForSeconds(_stopTime);
                _isMoving = true;
            }
            else
            {
                yield return new WaitForSeconds(_moveTime);
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if it reached the end of the screen
        if (collision.CompareTag(FINISH))
        {
            // Stop the spawner
            _bulletSystem.StopSpawner();
        }

        // Check if the collision is with a bullet
        if (collision.CompareTag(BULLET_TAG))
        {
            // Take damage
            StartCoroutine(TakeDamage());
        }

        // Check if the collision is with the pool
        if (collision.CompareTag(POOL_TAG))
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
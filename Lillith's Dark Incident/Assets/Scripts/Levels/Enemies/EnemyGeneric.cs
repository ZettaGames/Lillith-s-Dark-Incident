using System.Collections;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour
{
    // CONSTANTS
    private const string BulletTag = "Bullet";
    private const string SuperTag = "Super";
    private const string PoolTag = "EnemyPool";
    private const string NoCollision = "NoCollision";
    private const string Finish = "Finish";
    private const string UNTAGGED = "Untagged";
    // private const string Check = "Check";

    // Enemy Settings
    [Header("Enemy Settings")]
    [SerializeField] private int _health;
    [SerializeField] protected float speed;

    // Enemy Drops
    [Header("Enemy Drops")]
    [SerializeField] private GameObject _scoreDrop;
    [SerializeField] private GameObject _starDrop;
    [SerializeField] private float _scoreDropChance = 0.3f;
    [SerializeField] private float _starDropChance = 0.1f;
    private float _dropRadius = 0.5f;
    private bool _scoreDropped = false;
    private bool _starDropped = false;

    // Components
    private SpriteRenderer _spriteRenderer;
    protected BulletSystem bulletSystem;

    private void Start()
    {
        // Initialization of components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSystem = GetComponent<BulletSystem>();
        bulletSystem.StartSpawner();

        // Start the behaviour
        StartCoroutine(Behaviour());
    }

    protected virtual IEnumerator Behaviour()
    {
        // To be overridden by the child class
        yield return null;
    }

    private IEnumerator TakeDamage()
    {
        // Reduce the health
        _health--;

        // Change the enemy color
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;

        // Check if the enemy is dead
        if (_health <= 0)
        {
            // Untag the enemy
            gameObject.tag = UNTAGGED;

            // Deactivate the enemy
            bulletSystem.StopSpawner();
            gameObject.tag = NoCollision;
            _spriteRenderer.enabled = false;

            // Drop bonus
            if (Random.value < _scoreDropChance && !_scoreDropped)
            {
                _scoreDropped = true;
                // Drop 3 score points in a triangle
                Instantiate(_scoreDrop, transform.position + Vector3.up * _dropRadius, Quaternion.identity);
                Instantiate(_scoreDrop, transform.position + Vector3.left * _dropRadius, Quaternion.identity);
                Instantiate(_scoreDrop, transform.position + Vector3.right * _dropRadius, Quaternion.identity);
            }

            if (Random.value < _starDropChance && !_starDropped)
            {
                _starDropped = true;
                Instantiate(_starDrop, transform.position + Vector3.down * _dropRadius, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the enemy is hit by a bullet
        if (collision.CompareTag(BulletTag) && !this.CompareTag(UNTAGGED))
        {
            // Update the score
            LevelScoreManager.Instance.EnemyHitBonus();

            // Take damage
            StartCoroutine(TakeDamage());
        }

        // Check if the enemy is hit by a super bullet
        if (collision.CompareTag(SuperTag) && !this.CompareTag(UNTAGGED))
        {
            // Update the score
            LevelScoreManager.Instance.SuperHitBonus();

            // Take damage
            _health = 1;
            StartCoroutine(TakeDamage());
        }

        // Check if the enemy reaches the end of the screen
        if (collision.CompareTag(Finish))
        {
            // Stop the spawner
            bulletSystem.StopSpawner();
        }

        // Check if the enemy is hit by the pool
        if (collision.CompareTag(PoolTag))
        {
            // Stop the spawner
            bulletSystem.InstantKill();

            // Destroy the enemy
            Destroy(gameObject);
        }
    }
}
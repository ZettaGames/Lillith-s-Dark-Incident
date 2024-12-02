using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassEnemyBehaviour : MonoBehaviour
{
    // Constant for the pool of enemies
    private const string POOL_TAG = "EnemyPool";

    // Enemy settings
    private int _health = 12;
    private float _speed = 2.0f;
    private float _speedMultiplier = 0.7f;

    // Bullet settings
    [SerializeField] private BulletSystem _bulletSystem;

    private void Start()
    {
        _bulletSystem.StartSpawner();
    }

    private void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime * LocalTime.TimeScale);
    }

    private void StartPhase2()
    {
        _speed *= _speedMultiplier;
    }

    private void OnEnable()
    {
        // Reset the enemys
        _health = 12;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        _bulletSystem.StartSpawner();

        GameEvents.OnPhaseTwoStart += StartPhase2;
    }

    private void OnDisable()
    {
        GameEvents.OnPhaseTwoStart -= StartPhase2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(POOL_TAG))
        {
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(TakeDamage());
        }
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
            // Deactivate renderer and collider
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            // Stop the bullet system
            _bulletSystem.StopSpawner();
        }
    }
}

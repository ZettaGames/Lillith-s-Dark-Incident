using System.Collections;
using UnityEngine;

public class ProjectilRock : MonoBehaviour
{
    // Constants
    private const string BREAK = "break";

    // Settings
    [Header("Rock Settings")]
    [SerializeField] private float _speed;

    // Components
    private SpriteRenderer _spriteRenderer;
    private BulletSystem _bulletSystem;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bulletSystem = GetComponent<BulletSystem>();
    }

    private void Update()
    {
        // Move the rock down
        transform.position += Vector3.down * _speed * Time.deltaTime * LocalTime.TimeScale;
    }

    // Destroy the rock into small pieces
    private IEnumerator Destroy()
    {
        _bulletSystem.StartSpawner();
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(4.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(BREAK))
        {
            StartCoroutine(Destroy());
        }
    }
}
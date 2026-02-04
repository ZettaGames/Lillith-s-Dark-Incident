using UnityEngine;

public class LillithSuper : MonoBehaviour
{
    private const string SUPER = "SuperPool";

    [Header("Super Attack")]
    [SerializeField] private float _speed;

    private void Update()
    {
        // Move the super attack up.
        transform.position += transform.up * _speed * Time.deltaTime * LocalTime.TimeScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the super attack when it collides with the pool or an enemy.
        if (other.CompareTag(SUPER))
        {
            Destroy(gameObject);
        }
    }
}

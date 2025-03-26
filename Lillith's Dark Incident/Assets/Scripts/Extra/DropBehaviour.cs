using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string ENEMY_POOL = "EnemyPool";

    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody2D.velocity = Vector2.down * _speed * LocalTime.TimeScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER_TAG) || collision.CompareTag(ENEMY_POOL))
        {
            Destroy(gameObject);
        }
    }
}
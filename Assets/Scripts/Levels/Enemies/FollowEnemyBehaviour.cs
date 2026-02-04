using System.Collections;
using UnityEngine;

public class FollowEnemyBehaviour : EnemyGeneric
{
    // Follow Settings
    [Header("Follow Settings")]
    [SerializeField] private float _followTime = 5.5f;
    [SerializeField] private float _followRadius = 3.5f;
    private float _followTimer = 0.0f;
    private Transform _followTarget;

    // Avoidance Settings
    [Header("Avoidance Settings")]
    [SerializeField] private float _avoidanceRadius = 1.5f;

    private void Awake()
    {
        // Set the target from the player layer
        _followTarget = GameObject.FindObjectOfType<LillithController>().transform;
    }

    protected override IEnumerator Behaviour()
    {
        while (_followTimer < _followTime)
        {
            if (LocalTime.TimeScale > 0.0f)
            {
                _followTimer += Time.deltaTime;
                FollowTarget();
            }
            yield return null;
        }

        // Stop the spawner and move the enemy down
        bulletSystem.StopSpawner();
        GetComponent<BoxCollider2D>().isTrigger = true;

        while (true)
        {
            if (LocalTime.TimeScale > 0.0f)
            {
                transform.position += Vector3.down * speed * Time.deltaTime * LocalTime.TimeScale;
            }
            yield return null;
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
            Vector3 moveDirection = direction.normalized * speed * Time.deltaTime;
            transform.position += moveDirection;
        }
        else if (distance < _followRadius - epsilon)
        {
            // Move the enemy away from the target
            Vector3 moveDirection = -direction.normalized * speed * Time.deltaTime;
            transform.position += moveDirection;
        }

        // Avoid other enemies
        AvoidOtherEnemies();
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
                Vector3 moveDirection = -directionToEnemy.normalized * speed * Time.deltaTime;
                transform.position += moveDirection;
            }
        }
    }
}
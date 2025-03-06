using System.Collections;
using UnityEngine;

public class PassEnemyBehaviour : EnemyGeneric
{
    // Settings
    [Header("Pass Settings")]
    [SerializeField] private float _moveTime;
    [SerializeField] private float _stopTime;
    [SerializeField] private float _stopChance;

    protected override IEnumerator Behaviour()
    {
        while (true)
        {
            float elapsedTime = 0f;
            while (elapsedTime < _moveTime)
            {
                transform.position += Vector3.down * speed * Time.deltaTime * LocalTime.TimeScale;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (Random.value < _stopChance)
            {
                yield return new WaitForSeconds(_stopTime);
            }
        }
    }
}
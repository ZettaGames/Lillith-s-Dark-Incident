using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossGeneric : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected Image _healthBar;
    protected int _currentHealth;

    [Header("States Settings")]
    [SerializeField] protected TMP_Text _stateText;
    [SerializeField] protected float _timeBetweenAttacks;
    protected float _timer;

    // Move the boss to the given position in the given time
    protected void MoveToDestination(Vector3 position, float time)
    {
        StartCoroutine(Move(position, time));
    }

    private IEnumerator Move(Vector3 position, float time)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, position, elapsedTime / time);
            elapsedTime += Time.deltaTime * LocalTime.TimeScale;
            yield return null;
        }

        transform.position = position;
    }

    private void TakeDamage()
    {
        StartCoroutine(Damage());
    }

    private void TakeSuperDamage()
    {
        StartCoroutine(SuperDamage());
    }

    private IEnumerator Damage()
    {
        // Decrease the health
        _currentHealth--;

        // Show damage in sprite
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        // Check if the boss is dead
        if (_currentHealth <= 0)
        {
            Death();
        }

    }

    private IEnumerator SuperDamage()
    {
        // Decrease the health
        _currentHealth -= 15 + (int)(_currentHealth * 0.05f);

        // Show damage in sprite
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;

        // Check if the boss is dead
        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        // To be overrided in children classes
    }

    // Choose between the given probabilities
    protected float Choose(float[] probs, int lastIndex)
    {
        Debug.Log("Eligiendo");

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;
        int selectedIndex = -1;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }

        // Ensure the selected index is not the same as the last index
        if (selectedIndex == lastIndex)
        {
            selectedIndex = (selectedIndex + 1) % probs.Length;
        }

        return probs.Length - 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Debug.Log(_currentHealth);
            TakeDamage();
        }

        if (collision.CompareTag("Super"))
        {
            Destroy(collision.gameObject);
            TakeSuperDamage();
        }
    }
}
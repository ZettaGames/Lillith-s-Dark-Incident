using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGlobalManager : MonoBehaviour
{
    private float _totalScore;
    private int _currentHealth;
    private int _maxHealth = 10;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _fullStar;
    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private TMP_Text _totalScoreText;

    private void Awake()
    {
        _totalScore = GameManager.Instance.TotalScore;
        _currentHealth = GameManager.Instance.CurrentStars;
    }

    private void Start()
    {
        // Fill the stars with the full star sprite
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].sprite = _fullStar;
        }

        StartCoroutine(ScoreScreen());
    }

    private IEnumerator ScoreScreen()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Empezando puntaje");

        // Make the score text to reach the total score
        while (_totalScoreText.text != $"{(int)_totalScore:D9}")
        {
            _totalScoreText.text = $"{(int)_totalScore:D9}";
            _totalScore += 500 * Time.deltaTime * LocalTime.TimeScale;
        }
        yield return new WaitForSeconds(2);

        // Change the stars based on the current health and update the multiplier
        for (int i = 0; i < _maxHealth; i++)
        {
            if (i < _currentHealth)
            {
                _stars[i].sprite = _fullStar;
            }
            else
            {
                _stars[i].sprite = _emptyStar;
            }
        }
        _multiplierText.text = $"{_currentHealth * 0.2f}";
        yield return new WaitForSeconds(2);

        // Multiply the total score by the multiplier
        _totalScore *= _currentHealth * 0.2f;

        // Make the total score text to reach the total score
        while (_totalScoreText.text != $"{(int)_totalScore:D9}")
        {
            _totalScoreText.text = $"{(int)_totalScore:D9}";
            _totalScore += 500 * Time.deltaTime * LocalTime.TimeScale;
        }
        yield return new WaitForSeconds(2);

        Debug.Log("Cambiarn nivel");
    }
}

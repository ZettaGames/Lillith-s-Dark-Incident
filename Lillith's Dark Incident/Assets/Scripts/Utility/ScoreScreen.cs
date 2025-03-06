using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    private float _totalScore;
    private float _tempScore = 0;
    private int _currentStars;
    private int _maxStars = 10;
    float _scoreTime = 5;

    [Header("Score Components")]
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI _multiplierText;
    [SerializeField] private TMPro.TextMeshProUGUI _totalScoreText;

    [Header("Health Components")]
    [SerializeField] private UnityEngine.UI.Image[] _stars;
    [SerializeField] private Sprite _fullStar;
    [SerializeField] private Sprite _emptyStar;

    private void Start()
    {
        // Set the time scale to 1
        LocalTime.TimeScale = 1f;

        // Get the total score and current stars from the GameManager
        _totalScore = GameManager.Instance.TotalScore;
        _tempScore = 0;
        _currentStars = GameManager.Instance.CurrentStars;

        if (_currentStars > 10)
        {
            _currentStars = 10;
        }

        // Fill the stars with the full star sprite
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].sprite = _fullStar;
        }

        Debug.Log("Puntaje total: " + _totalScore);
        Debug.Log("Estrellas actuales: " + _currentStars);
        Debug.Log("Puntaje temporal: " + _tempScore);

        StartCoroutine(Score());
    }

    private IEnumerator Score()
    {
        // Wait for 1.5 seconds before starting the score screen
        yield return new WaitForSeconds(1.5f);

        // Time to reach score
        float incrementTime = _totalScore / _scoreTime;

        Debug.Log("Iniciando puntaje");

        // Make the score text to reach the total score
        while (_tempScore < _totalScore)
        {
            _scoreText.text = $"{(int)_tempScore:D9}";
            _tempScore += incrementTime * Time.deltaTime * LocalTime.TimeScale;
            yield return null;
        }
        _scoreText.text = $"{(int)_totalScore:D9}";

        Debug.Log("Puntaje alcanzado");

        Debug.Log("Estrellas actuales: " + _currentStars);

        // Update the multiplier text based on the current stars
        for (int i = _maxStars; i >= _currentStars + 1; i--)
        {
            _stars[i - 1].sprite = _emptyStar;
            _multiplierText.text = $"{i * 0.2f - 0.2f}";
            yield return new WaitForSeconds(0.75f);
        }

        Debug.Log("Estrellas actualizadas");

        Debug.Log("Multiplicador: " + (_currentStars * 0.2f - 0.2f));

        // Multiply the total score by the multiplier
        _totalScore *= _currentStars * 0.2f;
        _tempScore = 0;

        // Make the total score text to reach the multiplied score
        while (_tempScore < _totalScore)
        {
            _totalScoreText.text = $"{(int)_tempScore:D9}";
            _tempScore += incrementTime * Time.deltaTime * LocalTime.TimeScale;
            yield return null;
        }
        _totalScoreText.text = $"{(int)_totalScore:D9}";

        Debug.Log("Puntaje total multiplicado");
    }
}
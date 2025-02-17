using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGlobalManager : MonoBehaviour
{
    private float _totalScore = 0.0f;
    private int _currentHealth;
    private int _maxHealth = 10;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _fullStar;
    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private TMP_Text _totalScoreText;

    public static ScoreGlobalManager Instance { get; private set; }

    public float TotalScore
    {
        get => _totalScore;
        set => _totalScore = value;
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Fill the stars with the full star sprite
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].sprite = _fullStar;
        }
    }

    public void StartScreen()
    {
        StartCoroutine(ScoreScreen());
    }

    private IEnumerator ScoreScreen()
    {
        Debug.Log("Empezando puntaje");
        transform.GetChild(0).gameObject.SetActive(true);

        // Make the score text to reach the total score
        while (_totalScoreText.text != $"{(int)_totalScore:D9}")
        {
            _totalScoreText.text = $"{(int)_totalScore:D9}";
            _totalScore += 500 * Time.deltaTime * LocalTime.TimeScale;
        }
        yield return new WaitForSeconds(2);

        // Change the stars based on the current health and update the multiplier
        while (_maxHealth != _currentHealth)
        {
            if (_currentHealth > 0)
            {
                // Change the sprite of the last star
                _stars[_currentHealth - 1].sprite = _emptyStar;
                _multiplierText.text = $"{_currentHealth * 0.2}";
                _currentHealth--;
            }
            else
            {
                break;
            }
        }
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
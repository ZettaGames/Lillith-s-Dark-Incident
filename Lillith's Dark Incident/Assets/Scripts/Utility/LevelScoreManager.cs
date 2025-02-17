using TMPro;
using UnityEngine;

public class LevelScoreManager : MonoBehaviour
{
    private float _score;
    private bool _onLevel = true;

    private int _currentHealth;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private LillithHealthManager _lillithHealthManager;

    public static LevelScoreManager Instance { get; private set; }

    public bool OnLevel
    {
        get => _onLevel;
        set => _onLevel = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _score = ScoreGlobalManager.Instance.TotalScore;
    }

    void Update()
    {
        _currentHealth = _lillithHealthManager.CurrentStars;

        _scoreText.text = $"{(int)_score:D9}";

        if (_onLevel)
        {
            // Constantly update the score text (200 per second)
            _score += 500 * Time.deltaTime * LocalTime.TimeScale;
        }
    }

    public void HitPenalty()
    {
        _score -= _score * 0.25f;
    }

    public void EnemyHitBonus()
    {
        _score += 100;
    }

    public void BossHitBonus()
    {
        _score += Mathf.Max(1500 - (_score * 0.1f), 100);
    }

    public void SaveScore()
    {
        ScoreGlobalManager.Instance.TotalScore = _score;
        ScoreGlobalManager.Instance.CurrentHealth = _currentHealth;
    }
}
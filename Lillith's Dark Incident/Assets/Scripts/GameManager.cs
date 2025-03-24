using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Levels
    private const int SkidLevel = 5;
    private const int DarkyLevel = 7;
    private const int LastCinematic = 9;
    private int _levelCounter = 1;

    // Score
    private float _totalScore;
    private int _currentStars;

    public float TotalScore
    {
        get => _totalScore;
        set => _totalScore = value;
    }

    public int CurrentStars
    {
        get => _currentStars;
        set => _currentStars = value;
    }

    // Singleton instance of the Game Manager
    public static GameManager Instance { get; private set; }

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
        // Lock the mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        var vsync = PlayerPrefs.GetInt("VSync", 1) == 1;
        SetFrameRate(vsync);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            LocalTime.TimeScale = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            LocalTime.TimeScale = 1.0f;
        }
#endif
    }

    public void SetFrameRate(bool vsync)
    {
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
        }

        if (!vsync)
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SetCurrentLevel(int level)
    {
        _levelCounter = level;
    }

    public int GetLevel()
    {
        _levelCounter++;
        if (_levelCounter == 2)
        {
            return SkidLevel;
        }
        else if (_levelCounter == 3)
        {
            return DarkyLevel;
        }
        else
        {
             return LastCinematic;
        }
    }
}
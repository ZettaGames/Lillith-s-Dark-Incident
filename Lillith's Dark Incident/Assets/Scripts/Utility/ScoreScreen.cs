using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    // CONSTANTS
    private const float ButtonDelay = 0.25f;
    private const float SpamDelay = 0.05f;
    private const string GoUp = "GoUp";
    private const string GoDown = "GoDown";

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

    [Header("Selection Buttons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private CanvasGroup _selectionCanvas;
    [SerializeField] private InputActionAsset _actionAsset;
    private InputActionMap _uiActionMap;

    [Header("Return PopOut")]
    [SerializeField] private Animator _returnPopOut;
    [SerializeField] private Button _resumeButton;

    private void Start()
    {
        // Set the time scale to 1
        LocalTime.TimeScale = 1f;

        _uiActionMap = _actionAsset.FindActionMap("UI");

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

        // Make the score text to reach the total score
        while (_tempScore < _totalScore)
        {
            _scoreText.text = $"{(int)_tempScore:D9}";
            _tempScore += incrementTime * Time.deltaTime * LocalTime.TimeScale;
            yield return null;
        }
        _scoreText.text = $"{(int)_totalScore:D9}";

        // Update the multiplier text based on the current stars
        for (int i = _maxStars; i >= _currentStars + 1; i--)
        {
            _stars[i - 1].sprite = _emptyStar;
            _multiplierText.text = $"{i * 0.2f - 0.2f}";
            yield return new WaitForSeconds(0.75f);
        }

        // Wait for 1.5 seconds before showing the total score
        yield return new WaitForSeconds(1.5f);

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

        // Wait for 1.5 seconds before showing the selection buttons
        yield return new WaitForSeconds(1.5f);

        // Fade in the selection buttons
        while (_selectionCanvas.alpha < 1)
        {
            _selectionCanvas.alpha += Time.deltaTime * LocalTime.TimeScale;
            yield return null;
        }

        // Select the continue button
        _continueButton.Select();
    }

    public void ContinueButton()
    {
        // Save the new total score
        GameManager.Instance.TotalScore = _totalScore;

        int nextLevel = GameManager.Instance.GetLevel();
        SceneTransitionManager.Instance.LoadLevel(nextLevel);
    }

    public void MainMenuButton()
    {
        StartCoroutine(MainMenuPopOut());
    }

    private IEnumerator MainMenuPopOut()
    {
        // Prevent button spam
        _uiActionMap.Disable();
        // Animation of the pop out
        _returnPopOut.SetTrigger(GoUp);
        // Wait to prevent button spam
        yield return new WaitForSeconds(SpamDelay);
        _resumeButton.Select();
        yield return new WaitForSeconds(ButtonDelay);
        _uiActionMap.Enable();
    }

    public void Return()
    {
        SceneTransitionManager.Instance.LoadLevel(2);
    }

    public void Resume()
    {
        StartCoroutine(ResumePopOut());
    }

    private IEnumerator ResumePopOut()
    {
        // Prevent button spam
        _uiActionMap.Disable();
        // Animation 
        _returnPopOut.SetTrigger(GoDown);
        // Wait to prevent button spam
        yield return new WaitForSeconds(SpamDelay);
        _continueButton.Select();
        yield return new WaitForSeconds(ButtonDelay);
        _uiActionMap.Enable();
    }
}
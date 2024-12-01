using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    // ! Constants
    private const string PAUSE_ACTION = "Pause";
    private const float BUTTON_DELAY = 0.25f;
    private const float SPAM_DELAY = 0.05f;

    private const string GO_UP = "GoUp";
    private const string GO_DOWN = "GoDown";

    [Header("Pause Menu Buttons")]
    [SerializeField] private Button _resumeButton;

    [Header("Components")]
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private Animator _pauseAnimator;
    private InputAction _pauseAction;

    private void Start()
    {
        // Initialization of the pause action
        _pauseAction = _actionAsset.FindActionMap("Player").FindAction(PAUSE_ACTION);
        if (_pauseAction != null)
        {
            _pauseAction.Enable();
        }
        else
        {
            Debug.LogError("Pause action not found in the InputActionAsset.");
        }
    }

    private void Update()
    {
        if (_pauseAction.triggered)
        {
            if (LocalTime.TimeScale == 0)
            {
                StartCoroutine(Resume());
            }
            else
            {
                StartCoroutine(Pause());
            }
        }
    }

    public void ResumeButton()
    {
        StartCoroutine(Resume());
    }

    public void MainMenuButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        LocalTime.TimeScale = 1;
        LevelLoader.Instance.LoadLevel(2);
    }

    private IEnumerator Pause()
    {
        // Pause the game
        LocalTime.TimeScale = 0;

        // Prevent button spam
        _actionAsset.Disable();
        // Animation of the pause menu
        _pauseAnimator.SetTrigger(GO_UP);
        // Wait to prevent button spam
        yield return new WaitForSeconds(SPAM_DELAY);
        EventSystem.current.SetSelectedGameObject(_resumeButton.gameObject);
        _actionAsset.Enable();
    }

    private IEnumerator Resume()
    {
        // Prevent button spam
        _actionAsset.Disable();
        // Wait to play button animation
        yield return new WaitForSeconds(BUTTON_DELAY);
        // Animation of the pause menu
        _pauseAnimator.SetTrigger(GO_DOWN);
        // Wait to prevent button spam
        yield return new WaitForSeconds(SPAM_DELAY);
        EventSystem.current.SetSelectedGameObject(null);
        _actionAsset.Enable();
        yield return new WaitForSeconds(BUTTON_DELAY);

        // Resume the game
        LocalTime.TimeScale = 1;
    }
}
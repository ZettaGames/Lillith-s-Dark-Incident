using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuTransitionsManager : MonoBehaviour
{
	#region variables
	// CONSTANTS
	// Animator triggers
	private const string GoUp = "GoUp";
	private const string GoDown = "GoDown";
	private const string GoLeft = "GoLeft";
	private const string GoRight = "GoRight";
	private const string GoIn = "GoIn";
	private const string GoOut = "GoOut";
	
	// Fade time
	private const float ButtonDelay = 0.25f;
	private const float SpamDelay = 0.05f;
	private const float SliderDelay = 0.1f;
	
	// General
	[Header("Misc")]
	[SerializeField] private float _fadeTime = 4f;
	[SerializeField] private InputActionAsset _actionAsset;
	private InputAction _submitAction;

	// Canvas of Main Menu and Settings Menu
	[Header("Canvas References")]
	[SerializeField] private CanvasGroup _mainCanvasGroup;
	[SerializeField] private CanvasGroup _settingsCanvasGroup;

	// Selectable buttons for Main Menu
	[Header("Main Menu Button References")]
	[SerializeField] private Button _newGameButton;
	[SerializeField] private Button _loadGameButton;
	[SerializeField] private Button _settingsButton;
	[SerializeField] private Button _exitButton;

	// Selectable buttons for Main Menu PopOuts
	[Header("Main PopOuts Button References")]
	[SerializeField] private Button _newGameYesButton;
	[SerializeField] private Button _loadGameNoButton;
	[SerializeField] private Button _exitNoButton;

	// Selectable buttons for Settings Menu
	[Header("Settings Menu Button References")]
	[SerializeField] private Button _soundButton;
    [SerializeField] private Button _screenButton;
    [SerializeField] private Button _returnButton;

	// Selectable buttons for Settings Menu PopOuts
	[Header("Settings PopOuts Buttons References")]
	[SerializeField] private Button _masterButton;
	[SerializeField] private Button _musicButton;
	[SerializeField] private Button _effectsButton;
    [SerializeField] private Button _fullscreenButton;

    // Sliders for Sound PopOut
    [Header("Settings Components")]
	[SerializeField] private Slider _masterSlider;
	[SerializeField] private Slider _musicSlider;
	[SerializeField] private Slider _effectsSlider;

    // POP-OUTS COMPONENTS
    // Logo
    [Header("Logo Animator")]
	[SerializeField] private Animator _logoAnimator;

	// Main Menu
	[Header("Main PopOuts Animator")]
	[SerializeField] private Animator _newGamePopOutAnimator;
	[SerializeField] private Animator _loadGamePopOutAnimator;
	[SerializeField] private Animator _exitPopOutAnimator;

	// Settings
	[Header("Settings PopOuts Animator")]
	[SerializeField] private Animator _soundPopOutAnimator;
	[SerializeField] private Animator _screenPopOutAnimator;
    #endregion

    #region unity_functions
    private void Start()
	{
		// Initialize the "Submit" action
		_submitAction = _actionAsset.FindActionMap("UI").FindAction("Submit");
	}

	private void Update()
	{
		// Swap to "master", "music" or "effects" button when on a slider
		if (EventSystem.current.currentSelectedGameObject == _masterSlider.gameObject && _submitAction.triggered)
		{
			EventSystem.current.SetSelectedGameObject(_masterButton.gameObject);
		}

		if (EventSystem.current.currentSelectedGameObject == _musicSlider.gameObject && _submitAction.triggered)
		{
			EventSystem.current.SetSelectedGameObject(_musicButton.gameObject);
		}

		if (EventSystem.current.currentSelectedGameObject == _effectsSlider.gameObject && _submitAction.triggered)
		{
			EventSystem.current.SetSelectedGameObject(_effectsButton.gameObject);
		}
	}
	#endregion

	#region main_settings_transitions
	public void SettingsButton()
	{
		StartCoroutine(SwitchCorroutines(GoRight, _mainCanvasGroup, _settingsCanvasGroup, _returnButton));
	}

	public void ReturnButton()
	{
		StartCoroutine(SwitchCorroutines(GoLeft, _settingsCanvasGroup, _mainCanvasGroup, _settingsButton));
	}
	#endregion

	#region main_popouts_transitions
	public void NewGameButton()
	{
		StartCoroutine(PopOutCorroutines(_newGamePopOutAnimator, GoUp, GoOut, false, _mainCanvasGroup, _newGameYesButton));
	}

	public void NewGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(_newGamePopOutAnimator, GoDown, GoIn, true, _mainCanvasGroup, _newGameButton));
	}

	public void LoadGameButton()
	{
		StartCoroutine(PopOutCorroutines(_loadGamePopOutAnimator, GoUp, GoOut, false, _mainCanvasGroup, _loadGameNoButton));
	}

	public void LoadGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(_loadGamePopOutAnimator, GoDown, GoIn, true, _mainCanvasGroup, _loadGameButton));
	}

	public void ExitButton()
	{
		StartCoroutine(PopOutCorroutines(_exitPopOutAnimator, GoUp, GoOut, false, _mainCanvasGroup, _exitNoButton));
	}

	public void ExitNoButton()
	{
		StartCoroutine(PopOutCorroutines(_exitPopOutAnimator, GoDown, GoIn, true, _mainCanvasGroup, _exitButton));
	}
	#endregion

	#region settings_popouts_transitions
	public void SoundButton()
	{
		StartCoroutine(PopOutCorroutines(_soundPopOutAnimator, GoUp, GoOut, false, _settingsCanvasGroup, _masterButton));
	}

	public void ScreenButton()
	{
		StartCoroutine(PopOutCorroutines(_screenPopOutAnimator, GoUp, GoOut, false, _settingsCanvasGroup, _fullscreenButton));
    }

	public void ReturnSoundButton()
	{
		StartCoroutine(PopOutCorroutines(_soundPopOutAnimator, GoDown, GoIn, true, _settingsCanvasGroup, _soundButton));
	}

    public void ReturnScreenButton()
    {
        StartCoroutine(PopOutCorroutines(_screenPopOutAnimator, GoDown, GoIn, true, _settingsCanvasGroup, _screenButton));
    }
    #endregion

    #region sound_sliders_transitions
    public void MoveToSliderMaster()
	{
		StartCoroutine(SelectSlider(_masterSlider));
	}

	public void MoveToSliderMusic()
	{
		StartCoroutine(SelectSlider(_musicSlider));
	}

	public void MoveToSliderEffects()
	{
		StartCoroutine(SelectSlider(_effectsSlider));
	}
	#endregion

	#region transitions_coroutines
	private IEnumerator PopOutCorroutines(Animator targetPopOut, string popOutMovement, string logoMovement, bool isFaded, CanvasGroup currentCanvas, Button finalButton)
	{
		// Prevent button spam
		_actionAsset.Disable();

		// Wait to play button animation
		yield return new WaitForSeconds(ButtonDelay);

		// Animations of menu
		targetPopOut.SetTrigger(popOutMovement);
		_logoAnimator.SetTrigger(logoMovement);
		EventSystem.current.SetSelectedGameObject(null);

		// Check if canvas are faded or not
		if (isFaded)
		{
			// Increase
			while (currentCanvas.alpha < 1)
			{
				currentCanvas.alpha += Time.deltaTime * _fadeTime;
				yield return null;
			}
		}
		else
		{
			// Decrease
			while (currentCanvas.alpha > 0)
			{
				currentCanvas.alpha -= Time.deltaTime * _fadeTime;
				yield return null;
			}
		}

		// Wait to prevent button spam
		yield return new WaitForSeconds(SpamDelay);
		_actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}

	private IEnumerator SwitchCorroutines(string logoDirection, CanvasGroup currentCanvas, CanvasGroup otherCanvas, Button finalButton)
	{
		// Prevent button spam
		_actionAsset.Disable();

		// Wait to play the button animations
		yield return new WaitForSeconds(ButtonDelay);
		_logoAnimator.SetTrigger(logoDirection);
		EventSystem.current.SetSelectedGameObject(null);

		// Fade-out current | Fade-in other
		while (currentCanvas.alpha > 0)
		{
			currentCanvas.alpha -= Time.deltaTime * _fadeTime;
			otherCanvas.alpha += Time.deltaTime * _fadeTime;
			yield return null;
		}

		// Wait to prevent button spam
		yield return new WaitForSeconds(SpamDelay);
		_actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}
	#endregion

	private IEnumerator SelectSlider(Slider target)
	{
		yield return new WaitForSeconds(SliderDelay);
		EventSystem.current.SetSelectedGameObject(target.gameObject);
	}
}
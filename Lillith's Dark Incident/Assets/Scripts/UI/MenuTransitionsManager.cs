using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuTransitionsManager : MonoBehaviour
{
	#region variables
	// ! Constants for animator triggers
	private const string GO_UP = "GoUp";
	private const string GO_DOWN = "GoDown";
	private const string GO_LEFT = "GoLeft";
	private const string GO_RIGHT = "GoRight";
	private const string GO_IN = "GoIn";
	private const string GO_OUT = "GoOut";
	
	// ! Constants for the fade time
	private const float BUTTON_DELAY = 0.25f;
	private const float SPAM_DELAY = 0.05f;
	private const float SLIDER_DELAY = 0.10f;
	
		
	// ! General
	[Header("Misc")]
	[SerializeField] private float _fadeTime = 4f;
	[SerializeField] private InputActionAsset _actionAsset;
	private InputAction _submitAction;

	// ! Canvas of Main Menu and Settings Menu
	[Header("Canvas References")]
	[SerializeField] private CanvasGroup _mainCanvasGroup;
	[SerializeField] private CanvasGroup _settingsCanvasGroup;

	// ! Selectable buttons for Main Menu
	[Header("Main Menu Button References")]
	[SerializeField] private Button _newGameButton;
	[SerializeField] private Button _loadGameButton;
	[SerializeField] private Button _settingsButton;
	[SerializeField] private Button _exitButton;

	// ! Selectable buttons for Main Menu PopOuts
	[Header("Main PopOuts Button References")]
	[SerializeField] private Button _newGameYesButton;
	[SerializeField] private Button _loadGameNoButton;
	[SerializeField] private Button _exitNoButton;

	// ! Selectable buttons for Settings Menu
	[Header("Settings Menu Button References")]
	[SerializeField] private Button _soundButton;
	[SerializeField] private Button _returnButton;

	// ! Selectable buttons for Settings Menu PopOuts
	[Header("Settings PopOuts Buttons References")]
	[SerializeField] private Button _masterButton;
	[SerializeField] private Button _musicButton;
	[SerializeField] private Button _effectsButton;

	// ! Sliders for Sound PopOut
	[Header("Settings Components")]
	[SerializeField] private Slider _masterSlider;
	[SerializeField] private Slider _musicSlider;
	[SerializeField] private Slider _effectsSlider;

	// ! PopOuts controllers
	// ? Logo
	[Header("Logo Animator")]
	[SerializeField] private Animator _logoAnimator;

	// ? Main Menu
	[Header("Main PopOuts Animator")]
	[SerializeField] private Animator _newGamePopOutAnimator;
	[SerializeField] private Animator _loadGamePopOutAnimator;
	[SerializeField] private Animator _exitPopOutAnimator;

	// ? Settings
	[Header("Settings PopOuts Animator")]
	[SerializeField] private Animator _soundPopOutAnimator;
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
		StartCoroutine(SwitchCorroutines(GO_RIGHT, _mainCanvasGroup, _settingsCanvasGroup, _returnButton));
	}

	public void ReturnButton()
	{
		StartCoroutine(SwitchCorroutines(GO_LEFT, _settingsCanvasGroup, _mainCanvasGroup, _settingsButton));
	}
	#endregion

	#region main_popouts_transitions
	public void NewGameButton()
	{
		StartCoroutine(PopOutCorroutines(_newGamePopOutAnimator, GO_UP, GO_OUT, false, _mainCanvasGroup, _newGameYesButton));
	}

	public void NewGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(_newGamePopOutAnimator, GO_DOWN, GO_IN, true, _mainCanvasGroup, _newGameButton));
	}

	public void LoadGameButton()
	{
		StartCoroutine(PopOutCorroutines(_loadGamePopOutAnimator, GO_UP, GO_OUT, false, _mainCanvasGroup, _loadGameNoButton));
	}

	public void LoadGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(_loadGamePopOutAnimator, GO_DOWN, GO_IN, true, _mainCanvasGroup, _loadGameButton));
	}

	public void ExitButton()
	{
		StartCoroutine(PopOutCorroutines(_exitPopOutAnimator, GO_UP, GO_OUT, false, _mainCanvasGroup, _exitNoButton));
	}

	public void ExitNoButton()
	{
		StartCoroutine(PopOutCorroutines(_exitPopOutAnimator, GO_DOWN, GO_IN, true, _mainCanvasGroup, _exitButton));
	}
	#endregion

	#region settings_popouts_transitions
	public void SoundButton()
	{
		StartCoroutine(PopOutCorroutines(_soundPopOutAnimator, GO_UP, GO_OUT, false, _settingsCanvasGroup, _masterButton));
	}

	public void ReturnSoundButton()
	{
		StartCoroutine(PopOutCorroutines(_soundPopOutAnimator, GO_DOWN, GO_IN, true, _settingsCanvasGroup, _soundButton));
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
		yield return new WaitForSeconds(BUTTON_DELAY);
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
		yield return new WaitForSeconds(SPAM_DELAY);
		_actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}

	private IEnumerator SwitchCorroutines(string logoDirection, CanvasGroup currentCanvas, CanvasGroup otherCanvas, Button finalButton)
	{
		// Prevent button spam
		_actionAsset.Disable();
		// Wait to play the button animations
		yield return new WaitForSeconds(BUTTON_DELAY);
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
		yield return new WaitForSeconds(SPAM_DELAY);
		_actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}
	#endregion

	private IEnumerator SelectSlider(Slider target)
	{
		yield return new WaitForSeconds(SLIDER_DELAY);
		EventSystem.current.SetSelectedGameObject(target.gameObject);
	}
}
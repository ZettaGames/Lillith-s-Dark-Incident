using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class MenuTransitionsManager : MonoBehaviour
{
	#region variables
	// ! General
	[Header("Misc")]
	[SerializeField] private float fadeTime = 4f;
	[SerializeField] private InputActionAsset actionAsset;
	private InputAction submitAction;
	
	// ! Canvas of Main Menu and Settings Menu
	[Header("Canvas References")]
	[SerializeField] private CanvasGroup mainCanvasGroup;
	[SerializeField] private CanvasGroup settingsCanvasGroup;
	
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
	[SerializeField] private Button _musicButton;
	[SerializeField] private Button _effectsButton;
	
	// ! Sliders for Sound PopOut
	[Header("Settings Components")]
	[SerializeField] private Slider _musicSlider;
	[SerializeField] private Slider _effectsSlider;
	
	// ! PopOuts controllers
	// ? Logo
	[Header("Logo Animator")]
	[SerializeField] private Animator logoAnimator;
	
	// ? Main Menu
	[Header("Main PopOuts Animator")]
	[SerializeField] private Animator newGamePopOutAnimator;
	[SerializeField] private Animator loadGamePopOutAnimator;
	[SerializeField] private Animator exitPopOutAnimator;
	
	// ? Settings
	[Header("Settings PopOuts Animator")]
	[SerializeField] private Animator soundPopOutAnimator;
	#endregion
	
	private void Start()
	{
		// Initialize the "Submit" action
		submitAction = actionAsset.FindActionMap("UI").FindAction("Submit");
	}
	
	private void Update()
	{	
		// Swap to "music" or "effects" button when on a slider
		if (EventSystem.current.currentSelectedGameObject == _musicSlider.gameObject && submitAction.triggered)
		{
			EventSystem.current.SetSelectedGameObject(_musicButton.gameObject);
		}
		
		if (EventSystem.current.currentSelectedGameObject == _effectsSlider.gameObject && submitAction.triggered)
		{
			EventSystem.current.SetSelectedGameObject(_effectsButton.gameObject);
		}
	}
	
	#region Main <-> Settings Transition
	public void SettingsButton()
	{
		StartCoroutine(SwitchCorroutines("GoRight", mainCanvasGroup, settingsCanvasGroup, _returnButton));
	}
	
	public void ReturnButton()
	{
		StartCoroutine(SwitchCorroutines("GoLeft", settingsCanvasGroup, mainCanvasGroup, _settingsButton));
	}
	#endregion
	
	#region Main <-> PopOuts Transitions
	public void NewGameButton()
	{
		StartCoroutine(PopOutCorroutines(newGamePopOutAnimator, "GoUp", "GoOut", false, mainCanvasGroup, _newGameYesButton));
	}
	
	public void NewGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(newGamePopOutAnimator, "GoDown", "GoIn", true, mainCanvasGroup, _newGameButton));
	}
	
	public void LoadGameButton()
	{
		StartCoroutine(PopOutCorroutines(loadGamePopOutAnimator, "GoUp", "GoOut", false, mainCanvasGroup, _loadGameNoButton));
	}
	
	public void LoadGameNoButton()
	{
		StartCoroutine(PopOutCorroutines(loadGamePopOutAnimator, "GoDown", "GoIn", true, mainCanvasGroup, _loadGameButton));
	}
	
	public void ExitButton()
	{
		StartCoroutine(PopOutCorroutines(exitPopOutAnimator, "GoUp", "GoOut", false, mainCanvasGroup, _exitNoButton));
	}
	
	public void ExitNoButton()
	{
		StartCoroutine(PopOutCorroutines(exitPopOutAnimator, "GoDown", "GoIn", true, mainCanvasGroup, _exitButton));
	}
	#endregion
	
	#region Settings <-> PopOuts Transitions
	public void SoundButton()
	{
		StartCoroutine(PopOutCorroutines(soundPopOutAnimator, "GoUp", "GoOut", false, settingsCanvasGroup, _musicButton));
	}
	
	public void ReturnSoundButton()
	{
		StartCoroutine(PopOutCorroutines(soundPopOutAnimator, "GoDown", "GoIn", true, settingsCanvasGroup, _soundButton));
	}

	#endregion
	
	#region Settings <-> Sound Sliders Transitions
	public void MoveToSliderMusic()
	{
		StartCoroutine(SelectSlider(_musicSlider));
	}
	
	public void MoveToSliderEffects()
	{
		StartCoroutine(SelectSlider(_effectsSlider));
	}
	#endregion
	
	#region Transition Corroutines
	private IEnumerator PopOutCorroutines(Animator targetPopOut, string popOutMovement, string logoMovement, bool isFaded, CanvasGroup currentCanvas, Button finalButton)
	{
		// Prevent button spam
		actionAsset.Disable();
		// Wait to play button animation
		yield return new WaitForSeconds(0.25f);
		// Animations of menu
		targetPopOut.SetTrigger(popOutMovement);
		logoAnimator.SetTrigger(logoMovement);
		EventSystem.current.SetSelectedGameObject(null);
		// Check if canvas are faded or not
		if (isFaded)
		{
			// Increase
			while (currentCanvas.alpha < 1)
			{
				currentCanvas.alpha += Time.deltaTime * fadeTime;
				yield return null;
			}
		}
		else
		{
			// Decrease
			while (currentCanvas.alpha > 0)
			{
				currentCanvas.alpha -= Time.deltaTime * fadeTime;
				yield return null;
			}
		}
		// Wait to prevent button spam
		yield return new WaitForSeconds(0.05f);
		actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}
	
	private IEnumerator SwitchCorroutines(string logoDirection, CanvasGroup currentCanvas, CanvasGroup otherCanvas, Button finalButton)
	{
		// Prevent button spam
		actionAsset.Disable();
		// Wait to play the button animations
		yield return new WaitForSeconds(0.25f);
		logoAnimator.SetTrigger(logoDirection);
		EventSystem.current.SetSelectedGameObject(null);
		// Fade-out current | Fade-in other
		while (currentCanvas.alpha > 0)
		{
			currentCanvas.alpha -= Time.deltaTime * fadeTime;
			otherCanvas. alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		// Wait to prevent button spam
		yield return new WaitForSeconds(0.05f);
		actionAsset.Enable();
		EventSystem.current.SetSelectedGameObject(finalButton.gameObject);
	}
	#endregion

	private IEnumerator SelectSlider(Slider target)
	{
		yield return new WaitForSeconds(0.10f);
		EventSystem.current.SetSelectedGameObject(target.gameObject);
	}
}
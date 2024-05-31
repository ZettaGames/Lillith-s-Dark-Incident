using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BackgroundImagesManager : MonoBehaviour
{
	// ! Misc variables
	private Button _currentButton;
	Dictionary<Button, Image> _buttonBackgrounds = new Dictionary<Button, Image>();

	// ! Buttons for the transition
	[Header("Main Menu Buttons")]
	[SerializeField] private Button _newGameButton;
	[SerializeField] private Button _loadGameButton;
	[SerializeField] private Button _settingsButton;
	[SerializeField] private Button _exitButton;

	[Header("Settings Menu Buttons")]
	[SerializeField] private Button _soundButton;
	[SerializeField] private Button _screenButton;
	[SerializeField] private Button _controlsButton;
	[SerializeField] private Button _returnButton;

	// ! Images for the transition
	[Header("Main Menu Backgrounds")]
	[SerializeField] private Image _newGameBackground;
	[SerializeField] private Image _loadGameBackground;
	[SerializeField] private Image _settingsBackground;
	[SerializeField] private Image _exitBackground;
	
	[Header("Settings Menu Backgrounds")]
	[SerializeField] private Image _soundBackground;
	[SerializeField] private Image _screenBackground;
	[SerializeField] private Image _controlsBackground;
	[SerializeField] private Image _returnMenuBackground;

	// ! Images for the load variants
	[Header("Load Variants")]
	[SerializeField] private Image _treeBackground;
	[SerializeField] private Image _squidBackground;
	[SerializeField] private Image _cloudBackground;

	// ! Transition variables
	[Header("Transition Settings")]
	[SerializeField] private float transitionTime = 0.5f;
	private float transitionTimer = 0f;
	private bool transitionCompleted = false;

	private void Awake()
	{
		// Add the buttons and their backgrounds to the dictionary
		_buttonBackgrounds.Add(_newGameButton, _newGameBackground);
		_buttonBackgrounds.Add(_loadGameButton, _loadGameBackground);
		_buttonBackgrounds.Add(_settingsButton, _settingsBackground);
		_buttonBackgrounds.Add(_exitButton, _exitBackground);
		_buttonBackgrounds.Add(_soundButton, _soundBackground);
		_buttonBackgrounds.Add(_screenButton, _screenBackground);
		_buttonBackgrounds.Add(_controlsButton, _controlsBackground);
		_buttonBackgrounds.Add(_returnButton, _returnMenuBackground);

		// Set the alpha of all backgrounds to 0
		foreach (var image in _buttonBackgrounds.Keys)
		{
			DecreaseAlpha(_buttonBackgrounds[image].color.a, 1);
		}
	}

	private void Update()
	{
		var previousButton = _currentButton;
		// Detect the current selected button
		try
		{
			_currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
		}
		catch (System.NullReferenceException)
		{
			_currentButton = null;
		}

		// If the current button is different from the previous one, reset the transition timer
		if (previousButton != _currentButton)
		{
			transitionTimer = 0;
			transitionCompleted = false;
		}

		// If it hasn't transitioned yet, call the transition method
		if (!transitionCompleted)
		{
			transitionTimer += Time.deltaTime;
			TransitionBackgrounds();
		}
	}

	// ! Transition controller
	private void TransitionBackgrounds()
	{
		// Change the alpha of the backgrounds if the corresponding button is selected
		foreach (var button in _buttonBackgrounds.Keys)
		{
			if (_buttonBackgrounds.TryGetValue(button, out var image))
			{
				var color = image.color;
				// If the button is the current one, increase the alpha, otherwise, decrease it
				color.a = (_currentButton != null && button == _currentButton) ? IncreaseAlpha(color.a) : DecreaseAlpha(color.a, transitionTimer);
				image.color = color;
			}
		}
	}

	// ! Increase the opacity
	private float IncreaseAlpha(float alpha)
	{
		alpha = Mathf.Lerp(0f, 1f, transitionTimer / transitionTime);
		alpha = Mathf.Clamp01(alpha);
		if (alpha >= 1f)
		{
			transitionCompleted = true;
		}
		return alpha;
	}

	// ! Decrease the opacity
	private float DecreaseAlpha(float alpha, float timer)
	{
		float localTimer = 0f;
		localTimer += Time.deltaTime;
		/* I have no clue on how multiplying the timer
		by a high number makes it work, but it does.*/
		alpha = Mathf.Lerp(alpha, 0f, localTimer * timer * 75 / transitionTime);
		/* Apparently, any number lower than 13 causes
		the image not to fade out completely. The higher
		the number, the faster the fade out. Incredible.*/
		alpha = Mathf.Clamp01(alpha);
		return alpha;
	}
}
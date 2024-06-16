using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IntroCinematic : MonoBehaviour
{
	// ! Constants for the cinematic direction
	private const int NEXT = 1;
	private const int PREVIOUS = -1;

	// ! Constant for the scene to load
	private const int FLOERA_LEVEL = 3;

	// ! Variables for the cinematic transition
	[SerializeField] private Image[] _images;
	[SerializeField] private float _delay = 0.75f;
	private int _currentIndex = 0;

	private void Start()
	{
		for (int i = 0; i < _images.Length; i++)
		{
			_images[i].gameObject.SetActive(false);
		}
		_images[0].gameObject.SetActive(true);
	}

	private void Update()
	{
		MoveForward();
		MoveBackward();
		SkipCinematic();
	}

	#region input_methods
	private void MoveForward()
	{
		var gamepad = Gamepad.current;

		// Transition to the next image
		if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame) || Keyboard.current[Key.RightArrow].wasPressedThisFrame)
		{
			// Load the next scene if the last image is reached
			if (_images[_currentIndex] == _images[_images.Length - 1])
			{
				// Load the scene and deactivate the cinematic
				LevelLoader.Instance.LoadLevel(FLOERA_LEVEL);
				gameObject.SetActive(false);
			}
			else
			{
				ShowImage(NEXT); // Load the next image
			}
		}
	}

	private void MoveBackward()
	{
		var gamepad = Gamepad.current;

		// Transition to the previous image
		if ((gamepad != null && gamepad.buttonWest.wasPressedThisFrame) || Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
		{
			// Only transition if the current image is not the first one
			if (_images[_currentIndex] != _images[0])
			{
				ShowImage(PREVIOUS); // Load the previous image
			}
		}
	}

	private void SkipCinematic()
	{
		var gamepad = Gamepad.current;

		// Skip the cinematic
		if ((gamepad != null && gamepad.buttonEast.wasPressedThisFrame) || Keyboard.current[Key.Escape].wasPressedThisFrame)
		{
			// Load the scene and deactivate the cinematic
			LevelLoader.Instance.LoadLevel(FLOERA_LEVEL);
			gameObject.SetActive(false);
		}
	}
	#endregion

	#region image_transition
	private void ShowImage(int direction)
	{
		// Fade out the current image
		Image currentImage = _images[_currentIndex];
		StartCoroutine(FadeImage(currentImage, 1.0f, 0.0f, _delay / 2.0f));
		
		// Update the current index
		_currentIndex = (_currentIndex + direction) % _images.Length;
		
		// Fade in the next image
		Image nextImage = _images[_currentIndex];
		nextImage.gameObject.SetActive(true);
		nextImage.color = new Color(nextImage.color.r, nextImage.color.g, nextImage.color.b, 0.0f); // set alpha to 0
		StartCoroutine(FadeImage(nextImage, 0.0f, 1.0f, _delay / 2.0f));
	}

	private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
	{
		// Get the current color of the image
		image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);
		
		// Calculate the rate of change
		float rate = 1.0f / duration;
		float elapsedTime = 0.0f;
		
		// Interpolate the alpha value
		while (elapsedTime < duration)
		{
			float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime * rate);
			image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		
		// Set the final alpha value of the image
		image.color = new Color(image.color.r, image.color.g, image.color.b, endAlpha);
	}
	#endregion
}
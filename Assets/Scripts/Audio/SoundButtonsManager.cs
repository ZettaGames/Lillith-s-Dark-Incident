using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButtonsManager : MonoBehaviour
{
	// Constant for the sound volume
	private const float Volume = 1f;
	
	// Audio clips to be played
	[Header("Audio Clips")]
	[SerializeField] private AudioClip _buttonClickAudio;
	[SerializeField] private AudioClip _buttonSelectAudio;

	// Last selected button
	private GameObject _lastSelected;
	
	private void Update()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

		// Play "buttonMove" clip when selecting a new button
		if (currentSelected != _lastSelected)
		{
			if (currentSelected != null)
			{
				// Play the sound from the SoundFXManager
				SoundFXManager.Instance.PlaySoundFXClip(_buttonSelectAudio, transform, Volume);
				
				// Update the current button
				_lastSelected = currentSelected;
			}
		}
	}

	// Method for playing "buttonClick" when pressing a new button (must be called from the button's OnClick event)
	public void PlaySoundButton()
	{
		SoundFXManager.Instance.PlaySoundFXClip(_buttonClickAudio, transform, Volume);
	}
}
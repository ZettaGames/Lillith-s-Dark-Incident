using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButtonsManager : MonoBehaviour
{
	[Header("Audio Clips")]
	[SerializeField] private AudioClip _buttonClickAudio;
	[SerializeField] private AudioClip _buttonSelectAudio;
	private GameObject _lastSelected;
	
	private void Update()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

		// Play "buttonMove" clip when selecting a new button
		if (currentSelected != _lastSelected)
		{
			if (currentSelected != null)
			{
				SoundFXManager.Instance.PlaySoundFXClip(_buttonSelectAudio, transform, 1f);
				// Update the current button
				_lastSelected = currentSelected;
			}
		}
	}

	// Method for playing "buttonClick" when pressing a new button
	public void PlaySoundButton()
	{
		SoundFXManager.Instance.PlaySoundFXClip(_buttonClickAudio, transform, 1f);
	}
}
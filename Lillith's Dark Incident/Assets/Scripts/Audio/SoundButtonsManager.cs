using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButtonsManager : MonoBehaviour
{
	[Header("Audio Clips")]
	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip buttonMove;
	private GameObject lastSelected;
	
	private void Update()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

		// Play "buttonMove" clip when selecting a new button
		if (currentSelected != lastSelected)
		{
			if (currentSelected != null)
			{
				SoundFXManager.Instance.PlaySoundFXClip(buttonMove, transform, 1f);
				// Update the current button
				lastSelected = currentSelected;
			}
		}
	}

	// Method for playing "buttonClick" when pressing a new button
	public void PlaySoundButton()
	{
		SoundFXManager.Instance.PlaySoundFXClip(buttonClick, transform, 1f);
	}
}
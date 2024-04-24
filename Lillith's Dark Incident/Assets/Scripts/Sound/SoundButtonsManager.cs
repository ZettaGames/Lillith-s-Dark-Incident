using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButtonsManager : MonoBehaviour
{
	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip buttonMove;
	private GameObject lastSelected;

	private void Update()
	{
		GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

		if (currentSelected != lastSelected)
		{
			if (currentSelected != null)
			{
				SoundFXManager.Instance.PlaySoundFXClip(buttonMove, transform, 1f);
				lastSelected = currentSelected;
			}
		}
	}

	public void PlaySoundButton()
	{
		SoundFXManager.Instance.PlaySoundFXClip(buttonClick, transform, 1f);
	}
}
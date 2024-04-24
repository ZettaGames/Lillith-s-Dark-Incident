using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenScrollManager : MonoBehaviour
{
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private GameObject[] menuButtons;

	void Start()
	{
		// OnSelect listener to all buttons
		foreach (var button in menuButtons)
		{
			var trigger = button.gameObject.AddComponent<EventTrigger>();
			var entry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
			entry.callback.AddListener((data) => { OnSelect((BaseEventData)data); });
			trigger.triggers.Add(entry);
		}
	}

	public void OnSelect(BaseEventData eventData)
	{
		for (int i = 0; i < menuButtons.Length; i++)
		{
			if (eventData.selectedObject == menuButtons[i].gameObject)
			{
				AdjustScrollView(i);
				break;
			}
		}
	}

	void AdjustScrollView(int buttonIndex)
	{
		float targetPosition = CalculateTargetPosition(buttonIndex);
		StartCoroutine(ScrollToPosition(targetPosition));
	}

	IEnumerator ScrollToPosition(float position)
	{
		float elapsedTime = 0;
		float totalTime = 0.25f;
		float currentPos = scrollRect.verticalNormalizedPosition;

		while (elapsedTime < totalTime)
		{
			elapsedTime += Time.deltaTime;
			float newPos = Mathf.SmoothStep(currentPos, position, elapsedTime / totalTime);
			scrollRect.verticalNormalizedPosition = newPos;
			yield return null;
		}

		// Update the position
		scrollRect.verticalNormalizedPosition = position;
	}

	float CalculateTargetPosition(int index)
	{
		// Adjust depending on the button
		switch (index)
		{
			case 0:
				return 1.0f; // Top
			case 1:
				return 0.66f; // Intermediate
			case 2:
				return 0.66f;
			case 3:
				return 0.33f; // Intermediate
			case 4:
				return 0.33f;
			case 5:
				return 0.0f; // Bottom
			default:
				return 1.0f;
		}
	}
}

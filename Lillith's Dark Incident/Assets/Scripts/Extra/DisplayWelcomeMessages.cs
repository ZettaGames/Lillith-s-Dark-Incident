using System.Collections;
using TMPro;
using UnityEngine;

public class DisplayWelcomeMessages : MonoBehaviour
{
	private int _randomTen;
	[SerializeField] private TMP_Text _welcomeMessage;

	private enum WelcomeMessages
	{
		HowdyMessage,
		MagicMessage,
		BulletMessage,
		DarkyMessage,
		SchoolMessage,
		ErrorMessage,
		WorkMessage,
		GlassesMessage,
		OrtographyMessage,
		VoiceMessage,
		MarianaMessage,
		VencesMessage
	}

	private void Start()
	{
		_randomTen = Random.Range(0, 100);
		StartCoroutine(DisplayRandomWelcomeMessage());
	}

	private IEnumerator DisplayRandomWelcomeMessage()
	{
		yield return new WaitForSeconds(0.75f);

        if (_randomTen != 10)
		{
			_welcomeMessage.text = GetRandomWelcomeMessage();
		}
		else if (_randomTen == 10)
		{
			_welcomeMessage.text = "10 is a lucky number: 10 months, 10 days a week, 10 aliens... Wait, what?";
		}

		while (_welcomeMessage.alpha < 1)
		{
			_welcomeMessage.alpha += Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(0.25f);

		while (_welcomeMessage.alpha > 0)
		{
			_welcomeMessage.alpha -= Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(0.5f);
	}

	private string GetRandomWelcomeMessage()
	{
		var randomMessage = (WelcomeMessages)Random.Range(0, 12);
		switch (randomMessage)
		{
			case WelcomeMessages.HowdyMessage:
				return "Howdy! ... I feel evil saying that.";
			case WelcomeMessages.MagicMessage:
				return "Let the magic be with you...";
			case WelcomeMessages.BulletMessage:
				return "It's like a bullet, but with hell.";
			case WelcomeMessages.DarkyMessage:
				return "\"Darky\" sounded better.";
			case WelcomeMessages.SchoolMessage:
				return "I wish there was a magic academy or so.";
			case WelcomeMessages.ErrorMessage:
				return "Error 404: Magic not found.";
			case WelcomeMessages.WorkMessage:
				return "Imagine if the artist was paid for this.";
			case WelcomeMessages.GlassesMessage:
				return "It was easier to be the boy with glasses.";
			case WelcomeMessages.OrtographyMessage:
				return "Imajine writin porperli, wat a dreem.";
			case WelcomeMessages.VoiceMessage:
				return "I swear the VA was treated humanely... kinda.";
			case WelcomeMessages.MarianaMessage:
				return "If life gives you lemons, drop out of the career.";
			case WelcomeMessages.VencesMessage:
				return "Pending message, Vences not working.";
			default:
				return "Is even possible to get here?";
		}
	}
}
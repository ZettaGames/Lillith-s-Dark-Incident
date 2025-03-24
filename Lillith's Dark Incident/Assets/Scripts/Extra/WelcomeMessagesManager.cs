using System.Collections;
using TMPro;
using UnityEngine;

public class WelcomeMessagesManager : MonoBehaviour
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
		DannyMessage,
		VencesMessage,
		MemeMessage,
		AriMessage,
		AndrosMessage,
		RoussMessageOne,
		RoussMessageTwo,
        MarianaMessage,
		SaulMessage,
		ValeriaMessage,
		KatiaMessage
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
		var randomMessage = (WelcomeMessages)Random.Range(0, 21);
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
			case WelcomeMessages.DannyMessage: // From: Danny Hernandez
				return "If life gives you lemons, drop out of the career.";
			case WelcomeMessages.VencesMessage: // From: Vences
                return "I hate this game.";
			case WelcomeMessages.MemeMessage: // From: Meme4K
				return "I S   T H A T   A   L I L L I T H   R E F E R E N C E ?!?!?!";
			case WelcomeMessages.AriMessage: // From: AriMichito
				return "Error, brain ain't braining. Please try again later. ^w^";
			case WelcomeMessages.AndrosMessage: // From: Andros
				return "Despite everything, it’s no longer you.";
			case WelcomeMessages.RoussMessageOne: // From: Rouss
				return "4096 pixels of pure pain";
            case WelcomeMessages.RoussMessageTwo: // From: Rouss
				return "Imagine getting paid to make pixel art.";
			case WelcomeMessages.MarianaMessage: // From: Mariana
				return "Let's go! ... I guess";
            case WelcomeMessages.SaulMessage: // From: Saul
				return "Error 404";
            case WelcomeMessages.ValeriaMessage: // From: Valeria
				return "VIIM  for the queen";
			case WelcomeMessages.KatiaMessage: // From: Katia
				return "Amidst the clutches of darkness, magic stands as the last salvation.";
            default:
				return "Is even possible to get here?";
		}
	}
}
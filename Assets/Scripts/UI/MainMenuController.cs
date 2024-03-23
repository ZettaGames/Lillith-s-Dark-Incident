using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MainMenuController : MonoBehaviour
{
	[Header("Misc Settings")]
	[SerializeField] private float fadeTime = 4f;
	
	[Header("Canvas References")]
	[SerializeField] private CanvasGroup mainCanvasGroup;
	[SerializeField] private CanvasGroup settingsCanvasGroup;
	
	[Header("Button References")]
	[SerializeField] private Button _settingsButton;
	[SerializeField] private Button _returnButton;
	[SerializeField] private Button _newGameButton;
	[SerializeField] private Button _newGameYesButton;
	[SerializeField] private Button _loadGameButton;
	[SerializeField] private Button _loadGameNoButton;
	[SerializeField] private Button _exitButton;
	[SerializeField] private Button _exitNoButton;
	
	[Header("Animator References")]
	[SerializeField] private Animator logoAnimator;
	[SerializeField] private Animator newGamePopOutAnimator;
	[SerializeField] private Animator loadGamePopOutAnimator;
	[SerializeField] private Animator exitPopOutAnimator;
	
	public void NewGameButton()
	{
		StartCoroutine(NewGamePopOut());
	}
	
	public void NewGameNoButton()
	{
		StartCoroutine(ReturnMainFromNewGame());
	}
	
	public void LoadGameButton()
	{
		StartCoroutine(LoadGamePopOut());
	}
	
	public void LoadGameNoButton()
	{
		StartCoroutine(ReturnMainFromLoadGame());
	}
	
	public void ExitButton()
	{
		StartCoroutine(ExitPopOut());
	}
	
	public void ExitNoButton()
	{
		StartCoroutine(ReturnMainFromExit());
	}
	
	public void SettingsButton()
	{
		StartCoroutine(SwitchToSettings());
	}
	
	public void ReturnButton()
	{
		StartCoroutine(SwitchToMainMenu());
	}
	
	private IEnumerator NewGamePopOut()
	{
		yield return new WaitForSeconds(0.25f);
		newGamePopOutAnimator.SetTrigger("GoUp");
		logoAnimator.SetTrigger("GoOut");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha > 0)
		{
			mainCanvasGroup.alpha -= Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_newGameYesButton.gameObject);
	}
	
	private IEnumerator ReturnMainFromNewGame()
	{
		yield return new WaitForSeconds(0.25f);
		newGamePopOutAnimator.SetTrigger("GoDown");
		logoAnimator.SetTrigger("GoIn");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha < 1)
		{
			mainCanvasGroup.alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_newGameButton.gameObject);
	}
	
	private IEnumerator LoadGamePopOut()
	{
		yield return new WaitForSeconds(0.25f);
		loadGamePopOutAnimator.SetTrigger("GoUp");
		logoAnimator.SetTrigger("GoOut");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha > 0)
		{
			mainCanvasGroup.alpha -= Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_loadGameNoButton.gameObject);
	}
	
	private IEnumerator ReturnMainFromLoadGame()
	{
		yield return new WaitForSeconds(0.25f);
		loadGamePopOutAnimator.SetTrigger("GoDown");
		logoAnimator.SetTrigger("GoIn");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha < 1)
		{
			mainCanvasGroup.alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_loadGameButton.gameObject);
	}
	
	private IEnumerator ExitPopOut()
	{
		yield return new WaitForSeconds(0.25f);
		exitPopOutAnimator.SetTrigger("GoUp");
		logoAnimator.SetTrigger("GoOut");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha > 0)
		{
			mainCanvasGroup.alpha -= Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_exitNoButton.gameObject);
	}
	
	private IEnumerator ReturnMainFromExit()
	{
		yield return new WaitForSeconds(0.25f);
		exitPopOutAnimator.SetTrigger("GoDown");
		logoAnimator.SetTrigger("GoIn");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha < 1)
		{
			mainCanvasGroup.alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_exitButton.gameObject);
	}
	
	private IEnumerator SwitchToSettings()
	{
		yield return new WaitForSeconds(0.25f);
		logoAnimator.SetTrigger("GoRight");
		EventSystem.current.SetSelectedGameObject(null);
		while (mainCanvasGroup.alpha > 0)
		{
			mainCanvasGroup.alpha -= Time.deltaTime * fadeTime;
			settingsCanvasGroup.alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_returnButton.gameObject);
	}
	
	private IEnumerator SwitchToMainMenu()
	{
		yield return new WaitForSeconds(0.25f);
		logoAnimator.SetTrigger("GoLeft");
		EventSystem.current.SetSelectedGameObject(null);
		while (settingsCanvasGroup.alpha > 0)
		{
			settingsCanvasGroup.alpha -= Time.deltaTime * fadeTime;
			mainCanvasGroup.alpha += Time.deltaTime * fadeTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.05f);
		EventSystem.current.SetSelectedGameObject(_settingsButton.gameObject);
	}
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstLevelTransition : MonoBehaviour
{
	// ! Constants for the menu transition
	private const int MAIN_MENU = 1;
	private const float INITIAL_DELAY = 1.5f;
	private const float TRANSITION_DELAY = 1.0f;

	// ! Transition-related variables
	[Header("Transition Settings")]
	[SerializeField] private Animator _transitionAnimator;
	[SerializeField] private Image _coverImage;

	private void Start()
	{
		StartCoroutine(LoadMainMenu());
	}

	private IEnumerator LoadMainMenu()
	{
		// De-activate the cover image after the delay
		yield return new WaitForSeconds(INITIAL_DELAY);
		_coverImage.gameObject.SetActive(false);

		// Load the main menu scene asynchronously
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MAIN_MENU);
		asyncLoad.allowSceneActivation = false;

		// Wait until the scene is almost loaded
		while (asyncLoad.progress < 0.9f)
		{
			yield return null;
		}

		// Start the transition animation
		_transitionAnimator.SetTrigger("StartTransition");
		yield return new WaitForSeconds(TRANSITION_DELAY);

		// Allow the scene to be activated
		asyncLoad.allowSceneActivation = true;
	}
}
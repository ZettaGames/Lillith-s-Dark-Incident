using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	// ! Constants for the level loader
	private const float INITIAL_DELAY = 0.25f;
	private const float TRANSITION_DELAY = 1f;

	// ! Animator reference for the transition
	[Header("Animator Reference")]
	[SerializeField] private Animator _transitionAnimator;

	// ! Singleton instance of the Level Loader
	public static LevelLoader Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	#region index_loading
	public void LoadLevel(int index)
	{
		StartCoroutine(Load(index));
	}

	private IEnumerator Load(int index)
	{
		yield return new WaitForSeconds(INITIAL_DELAY);

		// Load the scene asynchronously
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
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
	#endregion

	#region name_loading
	public void LoadLevel(string name)
	{
		StartCoroutine(Load(name));
	}

	private IEnumerator Load(string name)
	{
		yield return new WaitForSeconds(INITIAL_DELAY);

		// Load the scene asynchronously
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
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
	#endregion
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstLevelTransition : MonoBehaviour
{
	[SerializeField] private Animator transitionAnimator;
	[SerializeField] private Image coverImage;
	
	private void Start()
	{
		StartCoroutine(LoadMainMenu());
	}
	
	private IEnumerator LoadMainMenu()
	{
		yield return new WaitForSeconds(0.5f);
		coverImage.gameObject.SetActive(false);
		SceneManager.LoadSceneAsync(1).allowSceneActivation = false;
		transitionAnimator.SetTrigger("StartTransition");
		yield return new WaitForSeconds(0.8f);
		SceneManager.LoadScene(1);
	}
}
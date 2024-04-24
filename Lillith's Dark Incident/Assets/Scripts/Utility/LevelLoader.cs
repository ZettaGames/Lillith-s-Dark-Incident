using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] private Animator transitionAnimator;
	
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
	
	public void LoadLevel(int index)
	{
		StartCoroutine(Load(index));
	}
	
	private IEnumerator Load(int index)
	{
		yield return new WaitForSeconds(0.25f);
		SceneManager.LoadSceneAsync(index).allowSceneActivation = false;
		transitionAnimator.SetTrigger("StartTransition");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(index);
	}
}
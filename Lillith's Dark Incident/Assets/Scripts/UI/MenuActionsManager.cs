using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuActionsManager : MonoBehaviour
{
	// Constants
	private const int NewGameIndex = 3;
	private const string ExitSceneName = "ExitScene";
	
	public void NewGameYes()
	{
		StartCoroutine(StartNewGame());
	}
	
	public void ExitYes()
	{
		StartCoroutine(ExitTransition());
	}
	
	private IEnumerator StartNewGame()
	{
		EventSystem.current.SetSelectedGameObject(null);
		SceneTransitionManager.Instance.LoadLevel(NewGameIndex);
        yield return null;
	}
	
	private IEnumerator ExitTransition()
	{
		EventSystem.current.SetSelectedGameObject(null);
        SceneTransitionManager.Instance.LoadLevel(ExitSceneName);
        yield return null;
	}
}
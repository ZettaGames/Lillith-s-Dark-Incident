using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuActionsManager : MonoBehaviour
{
	public void NewGameYes()
	{
		StartCoroutine(LoadNewGame());
	}
	
	public void ExitYes()
	{
		StartCoroutine(ExitTransition());
	}
	
	private IEnumerator LoadNewGame()
	{
		EventSystem.current.SetSelectedGameObject(null);
		LevelLoader.Instance.LoadLevel(2);
		yield return null;
	}
	
	private IEnumerator ExitTransition()
	{
		EventSystem.current.SetSelectedGameObject(null);
		LevelLoader.Instance.LoadLevel("ExitScene");
		yield return null;
	}
}
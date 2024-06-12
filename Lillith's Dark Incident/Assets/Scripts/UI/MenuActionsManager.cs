using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuActionsManager : MonoBehaviour
{
	// ! Constants
	private const int NEW_GAME_INDEX = 2;
	private const string EXIT_SCENE_NAME = "ExitScene";
	
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
		LevelLoader.Instance.LoadLevel(NEW_GAME_INDEX);
		yield return null;
	}
	
	private IEnumerator ExitTransition()
	{
		EventSystem.current.SetSelectedGameObject(null);
		LevelLoader.Instance.LoadLevel(EXIT_SCENE_NAME);
		yield return null;
	}
}
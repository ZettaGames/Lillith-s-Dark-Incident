using System.Collections;
using UnityEngine;

public class MenuActionsManager : MonoBehaviour
{
	public void NewGameYes()
	{
		StartCoroutine(New());
	}
	
	public void ExitButton()
	{
		StartCoroutine(ExitTransition());
	}
	
	private IEnumerator New()
	{
		LevelLoader.Instance.LoadLevel(2);
		yield return null;
	}
	
	private IEnumerator ExitTransition()
	{
		LevelLoader.Instance.LoadLevel(3);
		yield return null;
	}
}

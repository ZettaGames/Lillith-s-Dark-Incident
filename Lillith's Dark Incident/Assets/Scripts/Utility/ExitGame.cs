using System.Collections;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(Exit());
	}
	
	private IEnumerator Exit()
	{
		yield return new WaitForSeconds(1f);
		Application.Quit();
	}
}
using System.Collections;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
	private const float EXIT_TIME = 1.5f;
	
	void Start()
	{
		StartCoroutine(Exit());
	}
	
	private IEnumerator Exit()
	{
		yield return new WaitForSeconds(EXIT_TIME);
		Application.Quit();
	}
}
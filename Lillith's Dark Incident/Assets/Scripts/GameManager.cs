using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

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

	private void Start()
	{
		// Lock the mouse cursor
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		// Set the target frame rate
		Application.targetFrameRate = 60;
	}
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// ! Singleton instance of the Game Manager
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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			LocalTime.TimeScale = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.F2))
		{
			LocalTime.TimeScale = 1.0f;
        }
    }
}
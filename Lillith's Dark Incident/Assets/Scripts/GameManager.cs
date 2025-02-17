using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is the score screen
        if (scene.name == "ScoreScreen")
        {
            // Start the score screen coroutine
            ScoreGlobalManager.Instance.StartScreen();
        }
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
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // CONSTANTS
    // Level loading
    private const float InitialDelay = 0.25f;
    private const float TransitionDelay = 1f;

    // Main menu transition
    private const int BlackSceneIndex = 1;
    private const int MainMenuIndex = 2;
    private const float MainMenuInitialDelay = 2.0f;

    // Triggers
    private const string StartTransitionTrigger = "StartTransition";

    // Componentes
    [Header("Components")]
    [SerializeField] private CanvasGroup _coverImage;
    [SerializeField] private Animator _transitionAnimator;

    // Singleton instance
    public static SceneTransitionManager Instance { get; private set; }

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
        if (IsOnBlackScene())
        {
            StartCoroutine(LoadMainMenu());
        }
    }

    #region Level Loading
    public void LoadLevel(int index)
    {
        StartCoroutine(Load(index));
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(Load(name));
    }

    private IEnumerator Load(int index)
    {
        yield return new WaitForSeconds(InitialDelay);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is fully loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Start the transition animation
        _transitionAnimator.SetTrigger(StartTransitionTrigger);
        yield return new WaitForSeconds(TransitionDelay);

        // Allow the scene to be activated
        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator Load(string name)
    {
        yield return new WaitForSeconds(InitialDelay);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is fully loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Start the transition animation
        _transitionAnimator.SetTrigger(StartTransitionTrigger);
        yield return new WaitForSeconds(TransitionDelay);

        // Allow the scene to be activated
        asyncLoad.allowSceneActivation = true;
    }
    #endregion

    private IEnumerator LoadMainMenu()
    {
        // De-activate the cover image after the delay
        yield return new WaitForSeconds(MainMenuInitialDelay);
        _coverImage.gameObject.SetActive(false);

        // Load the main menu scene asynchronously
        yield return Load(MainMenuIndex);
    }

    private bool IsOnBlackScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        return activeScene.buildIndex == BlackSceneIndex;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogoTransition : MonoBehaviour
{
    // Logo Image
    [SerializeField] private CanvasGroup _logoImage;

    // Const for the logo
    private const int BLACK_SCENE = 1;
    private const float INITIAL_DELAY = 1.75f;
    private const float FADE_TIME = 1.5f;
    private const float FADE_DURATION = 2.25f;

    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(INITIAL_DELAY);

        // Fade the logo in and out
        StartCoroutine(Fade(0f, 1f, FADE_TIME));
        yield return new WaitForSeconds(FADE_DURATION);
        StartCoroutine(Fade(1f, 0f, FADE_TIME));

        yield return new WaitForSeconds(INITIAL_DELAY);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(BLACK_SCENE);
        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            // Lerp the alpha value of the logo
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            _logoImage.alpha = alpha;
            yield return null;
        }
        _logoImage.alpha = endAlpha;
    }
}
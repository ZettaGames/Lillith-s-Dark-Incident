using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogoTransition : MonoBehaviour
{
    // Logo Image
    [SerializeField] private CanvasGroup _logoImage;

    // Const for the logo
    private const int SCENE_01 = 1;
    private const float FADE_DELAY = 1.0f;
    private const float FADE_DURATION = 1.5f;

    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(FADE_DELAY);

        StartCoroutine(Fade(0f, 1f, FADE_DELAY));

        yield return new WaitForSeconds(FADE_DURATION);

        StartCoroutine(Fade(1f, 0f, FADE_DELAY));

        yield return new WaitForSeconds(FADE_DURATION);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SCENE_01);
        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            _logoImage.alpha = alpha;
            yield return null;
        }
        _logoImage.alpha = endAlpha;
    }
}

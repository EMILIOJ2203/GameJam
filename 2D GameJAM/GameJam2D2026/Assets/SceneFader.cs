using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 0.35f;

    private bool isTransitioning;

    private void Awake()
    {
        // Por si se te olvida asignarlo: intenta encontrarlo en hijos
        if (fadeOverlay == null)
            fadeOverlay = FindObjectOfType<CanvasGroup>();
    }

    public void GoToGameJam2D()
    {
        FadeToScene("GameJam2D");
    }

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning || fadeOverlay == null) return;
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        fadeOverlay.blocksRaycasts = true; // bloquea clicks durante el fade
        yield return FadeRoutine(fadeOverlay.alpha, 1f, fadeOutDuration);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            // suavizado (S-curve)
            k = k * k * (3f - 2f * k);

            fadeOverlay.alpha = Mathf.Lerp(from, to, k);
            yield return null;
        }

        fadeOverlay.alpha = to;
    }
}

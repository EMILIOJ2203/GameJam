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
        // Intenta encontrar el CanvasGroup del FadeOverlay por nombre (más seguro)
        if (fadeOverlay == null)
        {
            var go = GameObject.Find("FadeOverlay");
            if (go != null) fadeOverlay = go.GetComponent<CanvasGroup>();
        }

        // Último recurso: cualquier CanvasGroup (no ideal, pero evita null)
        if (fadeOverlay == null)
            fadeOverlay = FindObjectOfType<CanvasGroup>();

        // Importantísimo: si está invisible, no debe bloquear clicks
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
            fadeOverlay.interactable = false;
        }
    }

    // --- BOTONES ---

    public void GoToGameJam2D()
    {
        FadeToScene("GameJam2D");
    }

    public void QuitGame()
    {
        if (isTransitioning) return;

        // Si hay overlay, hacemos fade antes de salir
        if (fadeOverlay != null)
            StartCoroutine(FadeAndQuit());
        else
            QuitNow();
    }

    // --- CORE ---

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

    private IEnumerator FadeAndQuit()
    {
        isTransitioning = true;

        fadeOverlay.blocksRaycasts = true;
        yield return FadeRoutine(fadeOverlay.alpha, 1f, fadeOutDuration);

        QuitNow();
    }

    private void QuitNow()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            fadeOverlay.alpha = to;
            yield break;
        }

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

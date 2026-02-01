using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeOutDuration = 0.35f;

    [Header("Credits")]
    [SerializeField] private GameObject creditsOverlay;

    private bool isTransitioning;

    private void Awake()
    {
        // Buscar FadeOverlay por nombre si no está asignado
        if (fadeOverlay == null)
        {
            var go = GameObject.Find("FadeOverlay");
            if (go != null) fadeOverlay = go.GetComponent<CanvasGroup>();
        }

        if (fadeOverlay == null)
            fadeOverlay = FindObjectOfType<CanvasGroup>();

        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
            fadeOverlay.interactable = false;
        }

        // Asegura que créditos inicien ocultos
        if (creditsOverlay != null)
            creditsOverlay.SetActive(false);
    }

    // =========================
    // BOTONES DE MENÚ
    // =========================

    public void GoToGameJam2D()
    {
        FadeToScene("GameJam2D");
    }

    public void QuitGame()
    {
        if (isTransitioning) return;

        if (fadeOverlay != null)
            StartCoroutine(FadeAndQuit());
        else
            QuitNow();
    }

    public void ShowCredits()
    {
        if (isTransitioning || creditsOverlay == null) return;
        creditsOverlay.SetActive(true);
    }

    public void HideCredits()
    {
        if (creditsOverlay == null) return;
        creditsOverlay.SetActive(false);
    }

    // =========================
    // CORE FADE
    // =========================

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning || fadeOverlay == null) return;
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        fadeOverlay.blocksRaycasts = true;
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

            // Ease in-out (suave indie)
            k = k * k * (3f - 2f * k);

            fadeOverlay.alpha = Mathf.Lerp(from, to, k);
            yield return null;
        }

        fadeOverlay.alpha = to;
    }
}

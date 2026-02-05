using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundAudioController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioFondo;
    public AudioClip clipMenu;
    public AudioClip clipJuego;
    public string escenaMenu = "menu";
    public string escenaJuego = "GameJam2D";
    public float duracionFade = 0.25f;
    public float volumenMenu = 1f;
    public float volumenJuego = 1f;

    private static BackgroundAudioController instancia;
    private double inicioDsp;
    private Coroutine fadeRoutine;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        inicioDsp = AudioSettings.dspTime;

        if (audioFondo == null)
        {
            audioFondo = GetComponent<AudioSource>();
        }

        if (audioFondo != null)
        {
            audioFondo.loop = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        AplicarClipPorEscena(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AplicarClipPorEscena(scene.name);
    }

    void AplicarClipPorEscena(string nombreEscena)
    {
        if (audioFondo == null) return;

        AudioClip clip = null;
        if (nombreEscena == escenaMenu)
        {
            clip = clipMenu;
        }
        else if (nombreEscena == escenaJuego)
        {
            clip = clipJuego;
        }

        if (clip == null)
        {
            audioFondo.Stop();
            return;
        }

        double transcurrido = AudioSettings.dspTime - inicioDsp;
        int muestras = clip.samples;
        int frecuencia = clip.frequency;
        int muestraObjetivo = muestras > 0 ? (int)((transcurrido * frecuencia) % muestras) : 0;

        float volumenClip = nombreEscena == escenaMenu ? volumenMenu : volumenJuego;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeToClip(clip, muestraObjetivo, volumenClip));
    }

    System.Collections.IEnumerator FadeToClip(AudioClip clip, int muestraObjetivo, float volumenFinal)
    {
        float volumenInicial = audioFondo.volume;
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = duracionFade <= 0f ? 1f : Mathf.Clamp01(tiempo / duracionFade);
            audioFondo.volume = Mathf.Lerp(volumenInicial, 0f, t);
            yield return null;
        }

        audioFondo.volume = 0f;
        audioFondo.clip = clip;
        audioFondo.timeSamples = Mathf.Clamp(muestraObjetivo, 0, Mathf.Max(0, clip.samples - 1));
        if (!audioFondo.isPlaying) audioFondo.Play();

        tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;
            float t = duracionFade <= 0f ? 1f : Mathf.Clamp01(tiempo / duracionFade);
            audioFondo.volume = Mathf.Lerp(0f, volumenFinal, t);
            yield return null;
        }

        audioFondo.volume = volumenFinal;
    }

    public void Play()
    {
        if (audioFondo != null) audioFondo.Play();
    }

    public void Stop()
    {
        if (audioFondo != null) audioFondo.Stop();
    }

    public void SetVolume(float volumen)
    {
        if (audioFondo != null) audioFondo.volume = Mathf.Clamp01(volumen);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement; // <-- IMPORTANTE: Necesario para cambiar de escena
using System.Collections;

public class MetaTrigger : MonoBehaviour
{
    public GameObject textoGanaste;
    private bool yaGano = false;

    // Tiempo que esperas viendo el mensaje "Ganaste" antes de ir al menú
    public float tiempoEsperaMenu = 4f;

    void Start()
    {
        if (textoGanaste != null)
            textoGanaste.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // El chivato para la consola
        UnityEngine.Debug.Log("CHOQUE DETECTADO CON: " + other.gameObject.name + " | TAG: " + other.tag);

        if (yaGano) return;

        if (other.CompareTag("Jugador"))
        {
            UnityEngine.Debug.Log("¡ES EL JUGADOR! ¡GANASTE!");
            yaGano = true;

            // 1. Mostrar texto visual
            if (textoGanaste != null)
                textoGanaste.SetActive(true);

            // 2. Detener físicas del jugador
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // En Unity 6 usa 'linearVelocity', en versiones viejas 'velocity'
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic; // Congelamos totalmente
            }

            // 3. Desactivar controles
            var mov = other.GetComponent<MovimientoJugador>();
            if (mov != null) mov.enabled = false;

            // 4. Iniciar la cuenta regresiva para volver al menú
            StartCoroutine(EsperarYCargarMenu());
        }
    }

    private IEnumerator EsperarYCargarMenu()
    {
        // Esperamos unos segundos para celebrar
        yield return new WaitForSeconds(tiempoEsperaMenu);

        // --- DEJAMOS LA NOTA MENTAL ---
        // Le decimos al SceneFader que la próxima vez active los créditos
        SceneFader.mostrarCreditosAlIniciar = true;

        // Cargamos la escena (Asegúrate de que se llame EXACTAMENTE así en tu carpeta)
        // Si tu escena se llama "MainMenu" o "Menu", cambia el texto de abajo
        SceneManager.LoadScene("menu");
    }
}
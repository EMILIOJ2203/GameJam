using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    public GameObject textoGanaste;

    private bool yaGano = false;

    void Start()
    {
        textoGanaste.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (yaGano) return;

        if (other.CompareTag("Jugador"))
        {
            yaGano = true;

            // Mostrar texto
            textoGanaste.SetActive(true);

            // Detener al jugador
            var mov = other.GetComponent<MovimientoJugador>();
            if (mov != null) mov.enabled = false;

            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }
}

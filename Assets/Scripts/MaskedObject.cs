using UnityEngine;

public class MaskedObject : MonoBehaviour
{
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Opcional: Que empiece un poco transparente para que parezca un "fantasma"
        Color c = sr.color;
        c.a = 0.5f; 
        sr.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        // Si el juego está en pausa (Modo Máscara activo) y hacemos clic
        if (Time.timeScale == 0 && Input.GetMouseButtonDown(0))
        {
            // Lanzamos un rayo desde la posición del mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                RevelarObjeto();
            }
        }
    }
    void RevelarObjeto()
    {
        // 1. Cambiamos el Layer a "Default" para que la Main Camera lo vea
        gameObject.layer = LayerMask.NameToLayer("Default");

        // 2. Le devolvemos la opacidad total
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        // 3. Quitamos la pausa y cerramos el modo máscara llamando al GameManager
        FindObjectOfType<GameController>().ToggleMaskMode();
        
        Debug.Log("Objeto revelado y añadido al mundo real.");
    }
}

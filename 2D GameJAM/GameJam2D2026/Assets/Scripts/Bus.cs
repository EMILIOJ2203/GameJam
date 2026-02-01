using UnityEngine;

public class MaskObject : MonoBehaviour
{
    private bool yaRevelado = false;
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        // Detectamos el clic sobre el colisionador del bus
        if (Time.timeScale == 0f && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!yaRevelado) {
                    Revelar();
                } else {
                    isDragging = true;
                    offset = transform.position - (Vector3)mousePos;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) isDragging = false;

        // Movimiento solo si ya fue revelado
        if (yaRevelado && isDragging && Time.timeScale == 0f)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Evita que el bus se pierda en el eje Z
            transform.position = (Vector2)mousePos + (Vector2)offset;
        }
    }

    void Revelar()
    {
        // 1. Primero volvemos el objeto real
        gameObject.layer = LayerMask.NameToLayer("Word");
        yaRevelado = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("Bus detectado y movido a capa Word");

        // 2. Buscamos el controlador
        GameController controller = FindObjectOfType<GameController>();
        if (controller != null)
        {
            // 3. SOLO AQUÍ quitamos la textura para que el Workspace quede limpio
            // Usamos el nombre del objeto que tiene tu Raw Image (Workspace)
            GameObject workspaceObj = GameObject.Find("Workspace"); 
            if (workspaceObj != null)
            {
                UnityEngine.UI.RawImage ri = workspaceObj.GetComponent<UnityEngine.UI.RawImage>();
                if (ri != null) ri.texture = null; // Se limpia al final
            }

            controller.ToggleMaskMode();
        }
    }
}
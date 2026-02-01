using UnityEngine;

public class MaskObject : MonoBehaviour
{
    private bool yaRevelado = false;
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;
    private PolygonCollider2D polyCol; // Usamos el tipo específico
    private SpriteRenderer sr;

    [Header("Configuración de Workspace")]
    public GameObject miWorkspace; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polyCol = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        
        // 1. Desactivamos el collider al inicio para que no choque con nada
        if (polyCol != null) polyCol.enabled = false; 
        
        // 2. Lo ponemos en Kinematic para que no caiga solo
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Detección de clic manual basada en el área del Sprite
        if (Time.timeScale == 0f && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (sr != null && sr.bounds.Contains(new Vector3(mousePos.x, mousePos.y, 0)))
            {
                if (miWorkspace != null && miWorkspace.activeSelf)
                {
                    if (!yaRevelado) Revelar();
                    
                    isDragging = true;
                    offset = transform.position - (Vector3)mousePos;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            if (isDragging)
            {
                isDragging = false;
                // 3. Al soltar, activamos el Polygon Collider y la gravedad
                //if (yaRevelado) ActivarFisicasDeApilado();
            }
        }

        if (isDragging && Time.timeScale == 0f)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            transform.position = (Vector2)mousePos + (Vector2)offset;
        }
    }

    void Revelar()
    {
        gameObject.layer = LayerMask.NameToLayer("Word");
        yaRevelado = true;
        if (sr != null) sr.color = Color.white;

        GameController controller = FindObjectOfType<GameController>();
        if (controller != null)
        {
            // Limpiamos la textura del Workspace asignado
            UnityEngine.UI.RawImage ri = miWorkspace.GetComponent<UnityEngine.UI.RawImage>();
            if (ri != null) ri.texture = null; 
            polyCol.enabled = true;
            
            controller.ToggleMaskMode();
        }
    }

    /*void ActivarFisicasDeApilado()
    {
        if (polyCol != null) polyCol.enabled = true; 
        
        if (rb != null) 
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 500f; // Masa alta para que el jugador no lo empuje

            // Cambiamos 'linearDrag' por 'drag' para evitar el error CS1061
            rb.linearDamping = 1f; 
        }
    }*/
}
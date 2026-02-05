using UnityEngine;

public class MaskObject : MonoBehaviour
{
    private bool yaRevelado = false;
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;
    private PolygonCollider2D polyCol; // Usamos el tipo específico
    private SpriteRenderer sr;
    private GameController controller;

    [Header("Configuración de Workspace")]
    public GameObject miWorkspace; 
    public bool debugClicks = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polyCol = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        controller = FindObjectOfType<GameController>();
        
        // 1. Desactivamos el collider al inicio para que no choque con nada
        if (polyCol != null) polyCol.enabled = false; 
        
        // 2. Lo ponemos en Kinematic para que no caiga solo
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // Detección de clic manual basada en el área del Sprite
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            if (sr != null && sr.bounds.Contains(mousePos))
            {
                if (!PuedeInteractuar(mousePos))
                {
                    if (debugClicks) Debug.Log("Click bloqueado: " + name);
                    return;
                }

                if (!yaRevelado) Revelar();

                isDragging = true;
                offset = transform.position - (Vector3)mousePos;
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

        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            transform.position = (Vector2)mousePos + (Vector2)offset;
        }
    }

    bool PuedeInteractuar(Vector3 mousePos)
    {
        if (controller != null && controller.IsMaskModeActive)
        {
            bool permitido = miWorkspace != null && miWorkspace.activeSelf;
            if (debugClicks) Debug.Log("MaskMode activo en " + name + " => " + permitido);
            return permitido;
        }

        bool esSuperior = EsSuperiorEnPunto(mousePos);
        if (debugClicks) Debug.Log("MaskMode inactivo en " + name + " => topmost: " + esSuperior);
        return esSuperior;
    }

    bool EsSuperiorEnPunto(Vector3 mousePos)
    {
        if (sr == null) return false;

        int miLayer = SortingLayer.GetLayerValueFromID(sr.sortingLayerID);
        int miOrden = sr.sortingOrder;

        MaskObject[] objetos = FindObjectsOfType<MaskObject>();
        foreach (MaskObject otro in objetos)
        {
            if (otro == this || otro.sr == null) continue;
            if (!otro.sr.bounds.Contains(mousePos)) continue;

            int otroLayer = SortingLayer.GetLayerValueFromID(otro.sr.sortingLayerID);
            int otroOrden = otro.sr.sortingOrder;

            if (otroLayer > miLayer)
            {
                if (debugClicks) Debug.Log("Bloqueado por layer: " + otro.name);
                return false;
            }
            if (otroLayer == miLayer && otroOrden > miOrden)
            {
                if (debugClicks) Debug.Log("Bloqueado por order: " + otro.name);
                return false;
            }
        }

        return true;
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

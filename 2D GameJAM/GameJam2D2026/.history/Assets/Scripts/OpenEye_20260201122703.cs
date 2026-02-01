using UnityEngine;
using UnityEngine.UI;

public class OpenEye : MonoBehaviour
{
    [Header("Configuración de Imágenes")]
    [SerializeField] 
    private Sprite ojoAbierto;
    
    [SerializeField] 
    private Sprite ojoCerrado;

    private Image imagenComponente;
    private bool estaAbierto = false;

    void Awake()
    {
        // En Unity 6, Awake es más seguro para inicializar componentes internos
        imagenComponente = GetComponent<Image>();
    }

    // Esta es la función que debes conectar al botón
    public void AlternarEstadoOjo()
    {
        if (imagenComponente == null) return;

        estaAbierto = !estaAbierto;

        if (estaAbierto)
        {
            imagenComponente.sprite = ojoAbierto;
        }
        else
        {
            imagenComponente.sprite = ojoCerrado;
        }
    }
}
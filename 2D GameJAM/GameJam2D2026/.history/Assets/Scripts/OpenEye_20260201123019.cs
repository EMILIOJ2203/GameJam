using UnityEngine;
using UnityEngine.UI;

public class OpenEye : MonoBehaviour
{
    [Header("Configuración de Imágenes")]
    public Sprite ojoAbierto;
    public Sprite ojoCerrado;

    private Image imagenComponente;
    private bool estaAbierto = false;

    void Start()
    {
        imagenComponente = GetComponent<Image>();

        if (imagenComponente != null && ojoCerrado != null)
        {
            imagenComponente.sprite = ojoCerrado;
        }
    }

    void Update()
    {
        // Actualización por frame si es necesario
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
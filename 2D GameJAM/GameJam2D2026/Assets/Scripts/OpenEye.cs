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

    public void AlternarEstadoOjo()
    {
        SetEstado(!estaAbierto);
    }

    public void SetEstado(bool abierto)
    {
        if (imagenComponente == null) return;

        estaAbierto = abierto;
        imagenComponente.sprite = estaAbierto ? ojoAbierto : ojoCerrado;
    }
}

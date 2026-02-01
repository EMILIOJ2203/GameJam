using UnityEngine;
using UnityEngine.UI; // Necesario para manejar componentes Image

public class OpenEye : MonoBehaviour
{
    public Sprite ojoAbierto;
    public Sprite ojoCerrado;

    private Image imagenUI;
    private bool estaAbierto = false;

    void Start()
    {
        // Obtenemos el componente Image del botón
        imagenUI = GetComponent<Image>();
        
        // Empezamos con el ojo cerrado por defecto
        if (ojoCerrado != null)
            imagenUI.sprite = ojoCerrado;
    }

    public void AlternarOjo()
    {
        estaAbierto = !estaAbierto;

        // Cambiamos el sprite según el estado
        if (estaAbierto)
            imagenUI.sprite = ojoAbierto;
        else
            imagenUI.sprite = ojoCerrado;
    }
}
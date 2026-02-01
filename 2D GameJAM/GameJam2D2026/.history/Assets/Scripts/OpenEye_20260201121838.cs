using UnityEngine;
using UnityEngine.UI; // Esto es vital para que reconozca "Image" y "Sprite"

public class OpenEye : MonoBehaviour
{
    // Estas líneas harán que aparezcan los cuadros en el Inspector
    public Sprite ojoAbierto;
    public Sprite ojoCerrado;

    private Image imagenComponente;

    void Awake()
    {
        imagenComponente = GetComponent<Image>();
    }

    public void AlternarOjo()
    {
        // Si el sprite actual es el cerrado, ponemos el abierto y viceversa
        if (imagenComponente.sprite == ojoCerrado)
        {
            imagenComponente.sprite = ojoAbierto;
        }
        else
        {
            imagenComponente.sprite = ojoCerrado;
        }
    }
}
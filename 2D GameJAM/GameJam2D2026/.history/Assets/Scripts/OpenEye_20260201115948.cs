using UnityEngine;

public class OpenEye : MonoBehaviour
{
    [Header("Configuración de Texturas")]
    public Texture texturaAbierto;
    public Texture texturaCerrado;

    private Renderer _renderer;
    private bool estaAbierto = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        // Empezamos con el ojo cerrado
        if (texturaCerrado != null)
            _renderer.material.mainTexture = texturaCerrado;
    }

    // Este método detecta el clic si el objeto tiene un Collider
    void OnMouseDown()
    {
        AlternarOjo();
    }

    public void AlternarOjo()
    {
        estaAbierto = !estaAbierto;

        if (estaAbierto)
        {
            _renderer.material.mainTexture = texturaAbierto;
            Debug.Log("Ojo Abierto");
        }
        else
        {
            _renderer.material.mainTexture = texturaCerrado;
            Debug.Log("Ojo Cerrado");
        }
    }
}

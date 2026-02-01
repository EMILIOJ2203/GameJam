using UnityEngine;

public class GameController : MonoBehaviour
{
    // Arrastra aquí la Raw Image grande que pusiste sobre el workspace
    public GameObject rawImageCentral; 
    
    private bool isMaskModeActive = false;

    void Start() 
    {
        // Al empezar, la capa central está oculta
        if(rawImageCentral != null) rawImageCentral.SetActive(false);
    }

    public void ToggleMaskMode() 
    {
        isMaskModeActive = !isMaskModeActive;
        
        // Activamos/Desactivamos la visualización en el workspace
        if(rawImageCentral != null)
        {
            rawImageCentral.SetActive(isMaskModeActive);
        }

        // Efecto Photoshop: Pausamos el tiempo para "editar" la capa
        Time.timeScale = isMaskModeActive ? 0f : 1f;
        
        Debug.Log("Modo Capa: " + isMaskModeActive);
    }
}

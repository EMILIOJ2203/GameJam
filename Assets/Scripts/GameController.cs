using UnityEngine;

public class GameController: MonoBehaviour
{
    // Arrastra aquí el objeto "Masked Camera" desde la jerarquía
    public GameObject maskedCamera; 
    
    private bool isMaskModeActive = false;

    void Start()
    {
        // Nos aseguramos de que empiece desactivada
        if(maskedCamera != null) maskedCamera.SetActive(false);
    }

    public void ToggleMaskMode()
    {
        isMaskModeActive = !isMaskModeActive;
        
        // Esto activa/desactiva la cámara Overlay
        if(maskedCamera != null)
        {
            maskedCamera.SetActive(isMaskModeActive);
        }

        // Efecto Photoshop: El juego se pausa para que puedas "editar" la capa
        Time.timeScale = isMaskModeActive ? 0f : 1f;
        
        Debug.Log("Modo Máscara: " + isMaskModeActive);
    }
}
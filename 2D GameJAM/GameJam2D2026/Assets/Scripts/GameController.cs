using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Configuración de Capas")]
    // Arrastra aquí todos tus Workspaces (Workspace, Workspace-2, etc.)
    public GameObject[] todosLosWorkspaces; 
    
    private bool isMaskModeActive = false;
    private int capaActualIdx = -1;

    void Start() 
    {
        // Al empezar, nos aseguramos de que todos los Workspaces estén apagados
        foreach (GameObject ws in todosLosWorkspaces)
        {
            if(ws != null) ws.SetActive(false);
        }
    }

    // Esta función la llamarán tus botones de la derecha (0, 1, 2, 3...)
    public void ActivarCapa(int indice)
    {
        // 1. Si ya estamos en modo edición y pulsamos la misma capa, la cerramos
        if (isMaskModeActive && capaActualIdx == indice)
        {
            ToggleMaskMode();
            return;
        }

        // 2. Apagamos todas las capas primero para que no se encimen
        foreach (GameObject ws in todosLosWorkspaces)
        {
            ws.SetActive(false);
        }

        // 3. Encendemos solo la que elegimos
        capaActualIdx = indice;
        todosLosWorkspaces[indice].SetActive(true);
        
        // 4. Forzamos el modo edición (Pausa)
        isMaskModeActive = true;
        Time.timeScale = 0f;
        
        Debug.Log("Editando: " + todosLosWorkspaces[indice].name);
    }

    // Esta función la sigue llamando el Bus (MaskObject) para cerrar todo al revelar
    public void ToggleMaskMode() 
    {
        isMaskModeActive = !isMaskModeActive;
        
        // Si desactivamos el modo, apagamos todos los Workspaces
        if (!isMaskModeActive)
        {
            foreach (GameObject ws in todosLosWorkspaces)
            {
                ws.SetActive(false);
            }
            capaActualIdx = -1;
        }

        Time.timeScale = isMaskModeActive ? 0f : 1f;
    }
}

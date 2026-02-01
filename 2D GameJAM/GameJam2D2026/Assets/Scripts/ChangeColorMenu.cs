using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeColorMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private Color originalColor;

    [Range(1f, 2f)]
    public float brillo = 1.3f;

    void Start()
    {
        image = GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("❌ No hay Image en este objeto", this);
            enabled = false;
            return;
        }

        originalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = originalColor * brillo;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = originalColor;
    }
}

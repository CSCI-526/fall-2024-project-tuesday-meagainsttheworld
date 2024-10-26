using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI buttonText;
    [SerializeField] private Color normalColor = Color.black;
    [SerializeField] private Color hoverColor = Color.white;
    private bool isSelected = false;

    void Start()
    {
        // Set the initial color of the text.
        buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) buttonText.color = normalColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.color = hoverColor;
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.color = normalColor;
        isSelected = false;
    }
}
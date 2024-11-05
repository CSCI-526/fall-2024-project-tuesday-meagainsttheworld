using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupController : MonoBehaviour
{
    public Toggle easyToggle;
    public Toggle mediumToggle;
    public Toggle hardToggle;

    private Color normalColor = new Color(0.65f, 0.63f, 0.63f, 1f); // A6A1A1 color
    private Color selectedColor = Color.white;

    private void Start()
    {
        // Set initial colors
        SetToggleColors();

        // Add listeners to update colors when toggles are clicked
        easyToggle.onValueChanged.AddListener(isOn => SetToggleColors());
        mediumToggle.onValueChanged.AddListener(isOn => SetToggleColors());
        hardToggle.onValueChanged.AddListener(isOn => SetToggleColors());
    }

    private void SetToggleColors()
    {
        // Update colors based on toggle state
        easyToggle.targetGraphic.color = easyToggle.isOn ? selectedColor : normalColor;
        mediumToggle.targetGraphic.color = mediumToggle.isOn ? selectedColor : normalColor;
        hardToggle.targetGraphic.color = hardToggle.isOn ? selectedColor : normalColor;
    }
}
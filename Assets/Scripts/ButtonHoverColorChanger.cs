using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverColorChanger : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Color normalColor = Color.black;
    public Color hoverColor = Color.red;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        // Set the initial color of the text.
        buttonText.color = normalColor;

        // Add event listeners for the button's hover states.
        button.onClick.AddListener(OnClick);

        // Add custom listeners for mouse enter and exit.
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((eventData) => { OnHoverEnter(); });

        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((eventData) => { OnHoverExit(); });

        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    private void OnHoverEnter()
    {
        buttonText.color = hoverColor;
    }

    private void OnHoverExit()
    {
        buttonText.color = normalColor;
    }

    private void OnClick()
    {
        // Handle button click if needed.
    }
}
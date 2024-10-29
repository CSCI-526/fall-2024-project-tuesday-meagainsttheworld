using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkingUI : MonoBehaviour
{
    public float blinkSpeed = 1f; // Speed of blinking
    public Image spacebarImage; // Reference to the UI Image element
    public TextMeshProUGUI uiText; // Reference to the TextMeshPro element

    private bool isVisible = true;

    void Start()
    {
        // Color newColor1 = spacebarImage.color;
        // Color newColor2 = spacebarImage.color;
        // newColor1.a = 0.3f; // Set this to your desired transparency level (0 = fully transparent, 1 = fully opaque)
        // newColor2.a = 0.7f;
        // spacebarImage.color = newColor1;
        // uiText.color = newColor2;
        // Start the blinking coroutine
        StartCoroutine(Blink());
    }

    System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            // Toggle visibility by changing alpha
            isVisible = !isVisible;
            if (spacebarImage != null)
            {
                Color newColor = spacebarImage.color;
                newColor.a = isVisible ? 1f : 0f; // 1f is fully visible, 0f is fully transparent
                spacebarImage.color = newColor;
            }

            if (uiText != null)
            {
                Color newColor = uiText.color;
                newColor.a = isVisible ? 1f : 0f; // 1f is fully visible, 0f is fully transparent
                uiText.color = newColor;
            }

            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    public void StopBlinking()
    {
        StopCoroutine(Blink());
        if (spacebarImage != null)
        {
            Color newColor = spacebarImage.color;
            newColor.a = 0f; // Make it fully transparent when stopped
            spacebarImage.color = newColor;
        }
        
        if (uiText != null)
        {
            Color newColor = uiText.color;
            newColor.a = 0f; // Make it fully transparent when stopped
            uiText.color = newColor;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class TrapProgressBar : MonoBehaviour
{
    public Image fillImage;
    // public Text remainingTimeText;
    public Color barColor = Color.green; // New field for bar color
    private Trap currentTrap;

    private void Start()
    {
        // Set the initial color of the fill image
        fillImage.color = barColor;
    }

    public void SetTrap(Trap trap)
    {
        currentTrap = trap;
    }

    private void Update()
    {
        if (currentTrap != null)
        {
            fillImage.fillAmount = 1 - currentTrap.Progress;
            // remainingTimeText.text = $"{currentTrap.RemainingTime:F1}s";
            // UpdateBarColor();
        }
    }

    // private void SetBarColor(Color newColor)
    // {
    //     barColor = newColor;
    //     fillImage.color = barColor;
    // }
    // private void UpdateBarColor()
    // {
    //     if (currentTrap != null)
    //     {
    //         float progress = currentTrap.Progress;
    //         if (progress < 0.3f)
    //             SetBarColor(Color.green);
    //         else if (progress < 0.7f)
    //             SetBarColor(Color.yellow);
    //         else
    //             SetBarColor(Color.red);
    //     }
    // }
}
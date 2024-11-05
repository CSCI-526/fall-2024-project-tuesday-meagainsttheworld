using UnityEngine;
using UnityEngine.UI;

public class ToggleInitializer : MonoBehaviour
{
    public Toggle mediumToggle; // Drag the Medium toggle here in the Inspector

    void Start()
    {
        // Ensure Medium toggle is turned on at start
        mediumToggle.isOn = false; // Temporarily set it to false
        mediumToggle.isOn = true;  // Set it back to true to refresh the state
    }
}
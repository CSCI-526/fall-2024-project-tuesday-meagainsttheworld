using UnityEngine;

public class SpacebarInputHandler : MonoBehaviour
{
    private BlinkingUI blinkingUI;

    void Start()
    {
        blinkingUI = GetComponent<BlinkingUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            blinkingUI.StopBlinking();
            gameObject.SetActive(false); // Hide the prompt
        }
    }
}
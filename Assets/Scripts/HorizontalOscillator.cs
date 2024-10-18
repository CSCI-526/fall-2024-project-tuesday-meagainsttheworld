using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalOscillator : MonoBehaviour
{
    // Public variables to control the speed and range of oscillation
    public float oscillationSpeed = 2f;  // Speed of oscillation
    public float oscillationRange = 3f;  // Horizontal distance from the start position

    // Internal variables to track the starting position
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position using a sine wave
        float offset = Mathf.Sin(Time.time * oscillationSpeed) * oscillationRange;

        // Update the object's position only on the x-axis
        transform.position = new Vector3(startPosition.x + offset, transform.position.y, transform.position.z);
    }
}

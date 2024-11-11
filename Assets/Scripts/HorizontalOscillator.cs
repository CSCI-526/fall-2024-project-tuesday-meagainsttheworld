using UnityEngine;

public class HorizontalOscillator : MonoBehaviour
{
    public enum OscillationType
    {
        Horizontal,
        Vertical
    }
    // Public variables to control the speed and range of oscillation
    public float oscillationSpeed = 2f;  // Speed of oscillation
    public float oscillationRange = 3f;  // Horizontal distance from the start position
    [SerializeField] private bool isSinusoidal = false;
    [SerializeField] private OscillationType oscillationType = OscillationType.Horizontal;

    private Rigidbody2D rb;
    private float currTime = 0;

    // Internal variables to track the starting position
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = oscillationType == OscillationType.Horizontal ? new(oscillationSpeed, 0) : new(0, oscillationSpeed);
    }

    void FixedUpdate()
    {
        if (isSinusoidal)
        {
            // Calculate the new velocity using a cosine wave (using derivative based on position)
            float targetVel = oscillationSpeed * oscillationRange * Mathf.Cos(oscillationSpeed * currTime);
            rb.velocity = oscillationType == OscillationType.Horizontal ? new(targetVel, 0) : new(0, targetVel);
            currTime += Time.fixedDeltaTime;
        }
        else
        {
            if (oscillationType == OscillationType.Horizontal)
            {
                if (transform.position.x <= startPosition.x - oscillationRange) rb.velocity = new(oscillationSpeed, 0);
                if (transform.position.x > startPosition.x + oscillationRange) rb.velocity = new(-oscillationSpeed, 0);
            }
            else
            {
                if (transform.position.y <= startPosition.y - oscillationRange) rb.velocity = new(0, oscillationSpeed);
                if (transform.position.y > startPosition.y + oscillationRange) rb.velocity = new(0, -oscillationSpeed);
            }
        }
    }
}

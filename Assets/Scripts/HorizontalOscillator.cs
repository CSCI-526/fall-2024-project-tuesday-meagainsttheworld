using UnityEngine;

public class HorizontalOscillator : MonoBehaviour
{
    // Public variables to control the speed and range of oscillation
    public float oscillationSpeed = 2f;  // Speed of oscillation
    public float oscillationRange = 3f;  // Horizontal distance from the start position
    [SerializeField] private bool isSinusoidal = false;

    private Rigidbody2D rb;
    private float currTime = 0;

    // Internal variables to track the starting position
    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new(oscillationSpeed, 0);
    }

    void FixedUpdate()
    {
        if (isSinusoidal)
        {
            // Calculate the new velocity using a cosine wave (using derivative based on position)
            float targetVel = oscillationSpeed * oscillationRange * Mathf.Cos(oscillationSpeed * currTime);
            rb.velocity = new(targetVel, 0);
            currTime += Time.fixedDeltaTime;
        }
        else
        {
            if (transform.position.x <= startPosition.x - oscillationRange) rb.velocity = new(oscillationSpeed, 0);
            if (transform.position.x > startPosition.x + oscillationRange) rb.velocity = new(-oscillationSpeed, 0);
        }
    }
}

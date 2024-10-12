using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGravityZone : MonoBehaviour
{
    // Change the gravity for objects entering this zone
    public bool rightwardGravity = true;  // True for rightward, false for leftward
    public float gravityStrength = 9.8f;
    // store the original gravity scale of objects entering the zone
    private Dictionary<Rigidbody2D, float> originalGravityScales = new Dictionary<Rigidbody2D, float>();
    private List<Rigidbody2D> objectsInZone = new List<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (!originalGravityScales.ContainsKey(rb))
            {
                originalGravityScales[rb] = rb.gravityScale;
            }
            // Disable the built-in gravity

            // New Comment by Ziang Qin:
			// This code can cause bugs so I commented it out.
            //rb.gravityScale = 0;

            if (!objectsInZone.Contains(rb))
            {
                objectsInZone.Add(rb);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset gravity behavior
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && originalGravityScales.ContainsKey(rb))
        {
            rb.gravityScale = originalGravityScales[rb];
            originalGravityScales.Remove(rb);
            objectsInZone.Remove(rb);
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rb in objectsInZone)
        {
            if (rb != null)
            {
                // Apply a continuous force to simulate horizontal gravity
                Vector2 horizontalGravity = rightwardGravity ? new Vector2(gravityStrength, 0) : new Vector2(-gravityStrength, 0);
                rb.AddForce(horizontalGravity, ForceMode2D.Force);
            }
        }
    }
}

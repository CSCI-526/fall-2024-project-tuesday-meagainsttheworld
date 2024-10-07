using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGraviyShift : MonoBehaviour
{
    // Flip only the current object's gravity 
    private Rigidbody2D rb;
    private bool isGravityFlipped = false;
    public bool flipGravityAtStart = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flipGravityAtStart)
        {
            rb.gravityScale = -1;
            isGravityFlipped = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Flip the gravity scale only for this object
            rb.gravityScale = isGravityFlipped ? 1 : -1;
            isGravityFlipped = !isGravityFlipped;
        }
    }
}

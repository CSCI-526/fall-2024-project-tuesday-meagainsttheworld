using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGravityShift : MonoBehaviour
{
    // Flip the global gravity upside-down
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, -Physics2D.gravity.y);
        }
    }
}

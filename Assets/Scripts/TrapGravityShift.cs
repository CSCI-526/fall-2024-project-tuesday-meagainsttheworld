using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGravityShift : Trap
{
    public Vector3 gravityDirection = Vector3.down; // Default gravity direction
    private Vector3 originalGravity;

    private void Awake()
    {
        originalGravity = Physics2D.gravity;
    }

    public override void ActivateSkill()
    {
        // Change gravity direction
        Physics2D.gravity = gravityDirection * Physics2D.gravity.magnitude;
        Debug.Log($"Gravity shifted to {Physics2D.gravity}");
    }

    public override void DeactivateSkill()
    {
        // Restore original gravity
        Physics2D.gravity = originalGravity;
        Debug.Log("Gravity restored to original direction");
    }

    private void OnDisable()
    {
        // Ensure gravity is restored if the trap is disabled
        Physics2D.gravity = originalGravity;
    }
}

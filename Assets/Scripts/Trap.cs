using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    // public bool isDangerous = true;
    // public bool isControllable = false;

    public bool moveHorizontally = false;
    public float leftLimit = 5f;
    public float rightLimit = 5f;
    public bool moveVertically = false;
    public float upLimit = 5f;
    public float downLimit = 5f;
    public float moveSpeed = 10.0f;
    protected Vector3 originalScale;
    protected Vector3 startingPosition;
    protected bool hasTransformed = false;
    public abstract void ActivateSkill();
    public abstract void DeactivateSkill();

    protected void Start()
    {
        originalScale = transform.localScale;
        startingPosition = transform.position;
    }
    public void Move(Vector2 direction)
    {
        Vector2 actualDir = direction;
        if (!moveHorizontally)
        {
            actualDir.x = 0;
        }
        if (!moveVertically)
        {
            actualDir.y = 0;
        }

        if (actualDir != Vector2.zero)
        {
            Debug.Log($"Trap {name} moving: {actualDir}");
            transform.Translate(actualDir * Time.deltaTime * moveSpeed);
            // limit the movement
            Vector3 clampedPosition = transform.localPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, startingPosition.x - leftLimit, startingPosition.x + rightLimit);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, startingPosition.y - downLimit, startingPosition.y + upLimit);
            transform.localPosition = clampedPosition;
        }
    }
    protected void ExpandHorizontally(float n = 2)
    {
        if (!hasTransformed)
        {
            // Double the width of the trap
            transform.localScale = new Vector3(originalScale.x * n, originalScale.y, originalScale.z);
            hasTransformed = true;
            Debug.Log($"{name} expanded to double width.");
        }
    }

    protected void Collapse()
    {
        if (hasTransformed)
        {
            transform.localScale = originalScale;
            hasTransformed = false;
            Debug.Log($"{name} collapsed back to original size.");
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    // public bool isDangerous = true;
    // public bool isControllable = false;

    // movement control
    public bool moveHorizontally = false;
    public float leftLimit = 5f;
    public float rightLimit = 5f;
    public bool moveVertically = false;
    public float upLimit = 5f;
    public float downLimit = 5f;
    public float moveSpeed = 10.0f;

    // timer
    public float timeLimit = 3.0f;
    protected float timeUsed = 0f;
    public float RemainingTime => Mathf.Max(0, timeLimit - timeUsed);
    public float Progress => timeUsed / timeLimit;

    // other data
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

    public void ActivateSkillTimed()
    {
        Debug.Log("Skill deactivated.");
        if (timeUsed < timeLimit)
        {
            ActivateSkill();
            timeUsed += Time.deltaTime;
            // If accumulated time reaches max duration, deactivate skill
            if (timeUsed >= timeLimit)
            {
                Debug.Log($"Time limit of {timeLimit} sec is used up. Deactivate skill.");
                DeactivateSkill();
            }
        }
        else
        {
            Debug.Log($"Time limit of {timeLimit} sec is used up. Cannot use skill.");
        }
    }

    public void ResetTimer()
    {
        timeUsed = 0f;
    }

    // skills and restoration
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

    protected void Disappear()
    {
        gameObject.SetActive(false);
    }

    protected void Reappear()
    {
        gameObject.SetActive(true);
    }
}

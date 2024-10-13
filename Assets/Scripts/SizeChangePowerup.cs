using UnityEngine;

public class SizeChange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GrowPowerup"))
        {
            transform.localScale = transform.localScale.x < 2 ? new Vector3(2, 2, 1): transform.localScale;
            Debug.Log("Grow Activated");
        }
        if (other.CompareTag("ShrinkPowerup"))
        {
            transform.localScale = transform.localScale.x > 0.5 ? new Vector3(0.5f, 0.5f, 1): transform.localScale;
            Debug.Log("Shrink Activated");
        }
    }
}

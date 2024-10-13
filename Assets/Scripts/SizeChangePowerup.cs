using UnityEngine;
using System.Collections;

public class SizeChange : MonoBehaviour
{
    // OLD CODE
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("GrowPowerup"))
    //     {
    //         transform.localScale = transform.localScale.x < 2 ? new Vector3(2, 2, 1): transform.localScale;
    //         Debug.Log("Grow Activated");
    //     }
    //     if (other.CompareTag("ShrinkPowerup"))
    //     {
    //         transform.localScale = transform.localScale.x > 0.5 ? new Vector3(0.5f, 0.5f, 1): transform.localScale;
    //         Debug.Log("Shrink Activated");
    //     }
    // }


    public GameObject otherPlayer; // Reference to the other player
    public float effectDuration = 10f; // Public duration for how long the effect should last

    private Vector3 originalSize; // Store the original size of this player
    private Vector3 otherPlayerOriginalSize; // Store the original size of the other player

    private void Start()
    {
        // Store the original sizes at the start
        originalSize = transform.localScale;
        otherPlayerOriginalSize = otherPlayer.transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Grow powerup logic
        if (other.CompareTag("GrowPowerup"))
        {
            // Increase this player's size
            transform.localScale = new Vector3(2, 2, 1);
            Debug.Log("Grow Activated");

            // Decrease the other player's size
            if (otherPlayer != null)
            {
                otherPlayer.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                Debug.Log("Other Player Shrink Activated");
            }

            // Start the coroutine to revert sizes after effectDuration
            StartCoroutine(RevertSizesAfterTime());
        }

        // Shrink powerup logic
        if (other.CompareTag("ShrinkPowerup"))
        {
            // Decrease this player's size
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            Debug.Log("Shrink Activated");

            // Increase the other player's size
            if (otherPlayer != null)
            {
                otherPlayer.transform.localScale = new Vector3(2, 2, 1);
                Debug.Log("Other Player Grow Activated");
            }

            // Start the coroutine to revert sizes after effectDuration
            StartCoroutine(RevertSizesAfterTime());
        }
    }

    // To revert the size changes after the specified effect duration
    private IEnumerator RevertSizesAfterTime()
    {
        // Wait for the effect duration to complete
        yield return new WaitForSeconds(effectDuration);

        // Revert both players to their original sizes
        transform.localScale = originalSize;
        if (otherPlayer != null)
        {
            otherPlayer.transform.localScale = otherPlayerOriginalSize;
        }

        Debug.Log("Size Reverted");
    }


}

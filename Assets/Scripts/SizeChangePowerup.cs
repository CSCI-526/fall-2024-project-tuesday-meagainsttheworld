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


    public float effectDuration = 5f; // Public duration for how long the effect should last
    public float sizeChangeValue = 1;

    void Start()
    {
        transform.localScale *= sizeChangeValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Grow powerup logic
        if (other.CompareTag("Player"))
        {
            GameObject mainPlayer = other.gameObject;
            GameObject otherPlayer = mainPlayer.GetComponent<PlayerController_K>().OtherPlayer;

            // Change player sizes and stats in opposite ways
            Debug.Log("Size Change Activated");

            mainPlayer.transform.localScale *= sizeChangeValue;
            mainPlayer.GetComponent<PlayerController_K>().baseGravity /= sizeChangeValue;
            mainPlayer.GetComponent<PlayerController_K>().maxFallSpeed /= sizeChangeValue;
            mainPlayer.GetComponent<PlayerController_K>().moveSpeed /= sizeChangeValue;

            otherPlayer.transform.localScale /= sizeChangeValue;
            otherPlayer.GetComponent<PlayerController_K>().baseGravity *= sizeChangeValue;
            otherPlayer.GetComponent<PlayerController_K>().maxFallSpeed *= sizeChangeValue;
            otherPlayer.GetComponent<PlayerController_K>().moveSpeed *= sizeChangeValue;

            // Start the coroutine to revert sizes after effectDuration and destroy powerup
            StartCoroutine(RevertSizesAfterTime(mainPlayer, otherPlayer));
        }
    }

    // To revert the size changes after the specified effect duration
    private IEnumerator RevertSizesAfterTime(GameObject mainPlayer, GameObject otherPlayer)
    {
        // Disable powerup collider and make it invisible
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(effectDuration);

        // Revert both players to their original sizes and stats
        mainPlayer.transform.localScale /= sizeChangeValue;
        mainPlayer.GetComponent<PlayerController_K>().baseGravity *= sizeChangeValue;
        mainPlayer.GetComponent<PlayerController_K>().maxFallSpeed *= sizeChangeValue;
        mainPlayer.GetComponent<PlayerController_K>().moveSpeed *= sizeChangeValue;

        otherPlayer.transform.localScale *= sizeChangeValue;
        otherPlayer.GetComponent<PlayerController_K>().baseGravity /= sizeChangeValue;
        otherPlayer.GetComponent<PlayerController_K>().maxFallSpeed /= sizeChangeValue;
        otherPlayer.GetComponent<PlayerController_K>().moveSpeed /= sizeChangeValue;

        Debug.Log("Size Reverted");
        Destroy(gameObject);
    }
}

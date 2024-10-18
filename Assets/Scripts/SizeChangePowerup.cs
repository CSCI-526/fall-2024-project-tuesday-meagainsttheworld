using UnityEngine;
using System.Collections;

public class SizeChange : MonoBehaviour
{
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
            PlayerController mainStats = other.gameObject.GetComponent<PlayerController>();
            PlayerController otherStats = mainStats.OtherPlayer.GetComponent<PlayerController>();

            // Change player sizes and stats in opposite ways
            Debug.Log("Size Change Activated");

            mainStats.gameObject.transform.localScale *= sizeChangeValue;
            mainStats.baseGravity /= sizeChangeValue;
            mainStats.maxFallSpeed /= sizeChangeValue;
            mainStats.moveSpeed /= sizeChangeValue;

            otherStats.gameObject.transform.localScale /= sizeChangeValue;
            otherStats.baseGravity *= sizeChangeValue;
            otherStats.maxFallSpeed *= sizeChangeValue;
            otherStats.moveSpeed *= sizeChangeValue;

            // Start the coroutine to revert sizes after effectDuration and destroy powerup
            StartCoroutine(RevertSizesAfterTime(mainStats, otherStats));
        }
    }

    // To revert the size changes after the specified effect duration
    private IEnumerator RevertSizesAfterTime(PlayerController mainStats, PlayerController otherStats)
    {
        // Disable powerup collider and make it invisible
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(effectDuration);

        // Revert both players to their original sizes and stats
        mainStats.gameObject.transform.localScale /= sizeChangeValue;
        mainStats.baseGravity *= sizeChangeValue;
        mainStats.maxFallSpeed *= sizeChangeValue;
        mainStats.moveSpeed *= sizeChangeValue;

        otherStats.gameObject.transform.localScale *= sizeChangeValue;
        otherStats.baseGravity /= sizeChangeValue;
        otherStats.maxFallSpeed /= sizeChangeValue;
        otherStats.moveSpeed /= sizeChangeValue;

        Debug.Log("Size Reverted");
        Destroy(gameObject);
    }
}

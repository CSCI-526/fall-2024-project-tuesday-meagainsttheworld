using UnityEngine;
using System.Collections;

public class SizeChange : MonoBehaviour
{
    [SerializeField] private float effectDuration = 5f; // Public duration for how long the effect should last
    [SerializeField] private float sizeChangeValue = 1;
    [SerializeField] private bool regenerating = true;

    void Start()
    {
        transform.localScale *= sizeChangeValue;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Grow powerup logic
        if (other.CompareTag("Player"))
        {
            Debug.Log("Size Change Activated");
            
            PlayerController mainStats = other.GetComponent<PlayerController>();
            PlayerController otherStats = mainStats.OtherPlayer;

            // Start the coroutine to revert sizes after effectDuration and destroy powerup
            StartCoroutine(RevertSizesAfterTime(mainStats, otherStats));
        }
    }

    // To revert the size changes after the specified effect duration
    private IEnumerator RevertSizesAfterTime(PlayerController mainStats, PlayerController otherStats)
    {
        // Disable powerup collider and make it transparent if regernating or invisible if not
        GetComponent<Collider2D>().enabled = false;

        if (regenerating) GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
        else GetComponent<SpriteRenderer>().enabled = false;

        // Change player sizes and stats in opposite ways
        ChangeSizes(mainStats, otherStats, sizeChangeValue, 1/sizeChangeValue);

        float timeSpent = effectDuration > 2 ? effectDuration - 1 : effectDuration * 0.8f;

        yield return new WaitForSeconds(timeSpent);
        float timeLeft = effectDuration - timeSpent;

        float interval = timeLeft / 10;
        Color mainColor = mainStats.GetComponent<SpriteRenderer>().color;
        Color otherColor = otherStats.GetComponent<SpriteRenderer>().color;

        bool colorToggle = true;

        while (timeLeft > 0)
        {
            if (colorToggle)
            {
                mainStats.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
                otherStats.GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
            }
            else
            {
                mainStats.GetComponent<SpriteRenderer>().color = mainColor;
                otherStats.GetComponent<SpriteRenderer>().color = otherColor;
            }
            colorToggle = !colorToggle;
            timeLeft -= interval;
            yield return new WaitForSeconds(interval);
        }

        // Revert both players to their original sizes and stats
        ChangeSizes(mainStats, otherStats, 1/sizeChangeValue, sizeChangeValue);

        Debug.Log("Size Reverted");

        if (regenerating)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.5f);
        }
        else Destroy(gameObject);
    }

    private void ChangeSizes(PlayerController mainStats, PlayerController otherStats, float mainSize, float otherSize)
    {
        mainStats.gameObject.transform.localScale *= mainSize;
        mainStats.PlayerTrail.widthMultiplier *= mainSize;
        mainStats.transform.GetChild(0).localScale *= mainSize;
        mainStats.PlayerRb.mass *= mainSize * 100;
        mainStats.baseGravity /= mainSize;
        mainStats.maxFallSpeed /= mainSize;
        mainStats.moveSpeed /= mainSize;

        otherStats.gameObject.transform.localScale *= otherSize;
        otherStats.transform.GetChild(0).localScale *= otherSize;
        otherStats.PlayerTrail.widthMultiplier *= otherSize;
        otherStats.PlayerRb.mass *= otherSize * 100;
        otherStats.baseGravity /= otherSize;
        otherStats.maxFallSpeed /= otherSize;
        otherStats.moveSpeed /= otherSize;

        if (sizeChangeValue > 1)
        {
            mainStats.jumpHeight *= mainSize;
            mainStats.airAdjustMultiplier *= mainSize;
        }
        else
        {
            otherStats.jumpHeight *= otherSize;
            otherStats.airAdjustMultiplier *= otherSize;
        }
    }
}

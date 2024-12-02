using UnityEngine;

public class TriggerAnimationOnReach : MonoBehaviour
{
    [Header("References")]
    public GameObject player1;
    public Canvas targetCanvas;
    public PowerupAnimation powerupAnimation;

    private void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player1 && !PowerupAnimation.hasAnimationPlayed)
        {
            Debug.Log("Player1 reached the target point. Opening Canvas!");
            targetCanvas.gameObject.SetActive(true);

            if (powerupAnimation != null)
            {
                powerupAnimation.StartAnimation();
            }
            else
            {
                Debug.LogError("PowerupAnimation reference is missing!");
            }
        }
    }
}
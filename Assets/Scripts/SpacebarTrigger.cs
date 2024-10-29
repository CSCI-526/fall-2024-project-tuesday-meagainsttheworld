using UnityEngine;

public class SpacebarTrigger : MonoBehaviour
{
    public GameObject spacebarPrompt; // Reference to the UI element
    private int playersInZone = 0; // Tracks the number of players that have entered the zone
    private bool player1PastEnd = false; // Tracks if player 1 has moved past the right end
    private bool player2PastEnd = false; // Tracks if player 2 has moved past the right end

    private BoxCollider2D boxCollider;

    private void Start()
    {
        // Get the BoxCollider2D component of the trigger zone
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spacebarPrompt != null) 
        {
            playersInZone++;
            spacebarPrompt.SetActive(true); // Show the prompt when any player enters
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spacebarPrompt != null)
        {
            playersInZone = Mathf.Max(playersInZone - 1, 0);
        }
    }

    private void Update()
    {
        // Get the right edge position of the BoxCollider2D zone
        float rightEnd = boxCollider.bounds.max.x;

        // Check if both players have moved past the right end of the zone
        if (playersInZone > 0)
        {
            player1PastEnd = CheckPlayerPastEnd("Player1ObjectName", rightEnd);
            player2PastEnd = CheckPlayerPastEnd("Player2ObjectName", rightEnd);

            // If either player has moved past the right end, deactivate the prompt
            if (player1PastEnd || player2PastEnd)
            {
                spacebarPrompt.SetActive(false);
            }
        }
    }

    // Helper function to check if a player has moved past the right end of the zone
    private bool CheckPlayerPastEnd(string playerName, float rightEnd)
    {
        GameObject player = GameObject.Find(playerName);
        if (player != null)
        {
            return player.transform.position.x > rightEnd;
        }
        return false;
    }
}
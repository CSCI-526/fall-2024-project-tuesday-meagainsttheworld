using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersGravityShift : MonoBehaviour
{
    // Reference to the players
    public GameObject player1;
    public GameObject player2;

    private PlayerController player1Controller;
    private PlayerController player2Controller;

    public bool flipGravityAtStartForPlayer2 = true;

    void Start()
    {
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();
        if (flipGravityAtStartForPlayer2)
        {
            FlipGravityForPlayer(player2);
        }
    }

    void Update()
    {
        // Check if either player is grounded
        if ((player1Controller.IsGrounded || player2Controller.IsGrounded) &&
            (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            // Flip the gravity scale for both players
            FlipGravityForPlayer(player1);
            FlipGravityForPlayer(player2);
        }
    }

    // Function to flip gravity for a specific player
    void FlipGravityForPlayer(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.gravityScale *= -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player1; // Reference to Player 1
    public PlayerController player2; // Reference to Player 2

    private Vector2 lastPositionPlayer1; // Store last position of Player 1
    private Vector2 lastPositionPlayer2; // Store last position of Player 2

    private float waitTimePlayer1; // Time counter for Player 1
    private float waitTimePlayer2; // Time counter for Player 2

    // Properties to expose wait times
    public float WaitTimePlayer1 => waitTimePlayer1;
    public float WaitTimePlayer2 => waitTimePlayer2;

    private void Start()
    {
        StartCoroutine(LogPlayerPositions());
    }

    private IEnumerator LogPlayerPositions()
    {
        while (true) // Infinite loop to log positions every 2 seconds
        {
            LogPositions();
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
        }
    }

    private void LogPositions()
    {
        Vector2 currentPositionPlayer1 = player1.transform.position;
        Vector2 currentPositionPlayer2 = player2.transform.position;

        // Log positions
        Debug.Log($"Player 1 Position: {currentPositionPlayer1}");
        Debug.Log($"Player 2 Position: {currentPositionPlayer2}");

        // Check Player 1's position
        if (currentPositionPlayer1 == lastPositionPlayer1) waitTimePlayer1 += 0.5f;
        // Check Player 2's position
        if (currentPositionPlayer2 == lastPositionPlayer2) waitTimePlayer2 += 0.5f;
        Debug.Log($"WaitTime: player1: {waitTimePlayer1} player 2: {waitTimePlayer2}");
        // Update last positions
        lastPositionPlayer1 = currentPositionPlayer1;
        lastPositionPlayer2 = currentPositionPlayer2;
    }
}
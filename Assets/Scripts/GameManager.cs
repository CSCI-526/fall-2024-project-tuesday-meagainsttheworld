using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public TextMeshProUGUI lifeText;
    //public TextMeshProUGUI TrapsText;
    public TextMeshProUGUI WinText;
    public static int playerLives = 3;       // Starting lives for "Me"
    //public static int trapCount = 1;         // Starting traps for "World"
    private bool hasGameReset = false;
    public bool isMeWinner = false;   // Track if "Me" wins
    public bool isWorldWinner = false; // Track if "World" wins
    private bool hasWon = false; //Track if "Me" has already won
    public string flagTag = "Flag";    // Tag for the right wall finish

    // Start is called before the first frame update
    void Start()
    {
        if (!hasGameReset)
        {
            playerLives = 3;  // Reset to initial lives
            //trapCount = 1;    // Reset to initial traps
            hasGameReset = true;  // Set the flag to prevent multiple resets
        }
        // Initialize lives and traps, if necessary
        Debug.Log("Game started. Player lives: " + playerLives);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLives <= 0 && !hasWon)
        {
            WorldWins();
        }
    }

    public void StartGame(){
        titleScreen.gameObject.SetActive(false);
        lifeText.text = "Lives: " + playerLives;
        //also need traps number here !
    }

    public void MeWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isMeWinner = true;
        playerLives++; // Reward 1 extra life
        Debug.Log("Me wins! Extra life granted. Current lives: " + playerLives);
        WinText.gameObject.SetActive(true);
        WinText.text = "Me Wins!";
        
        // Move to next level or any transition logic
    }

    // Call this when "World" wins (e.g., player runs out of lives)
    public void WorldWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isWorldWinner = true;
        //trapCount++; // Reward an extra trap
        Debug.Log("World wins!");
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Destroy(playerController.gameObject);  // Destroy the player object to prevent further interaction
        }
        WinText.gameObject.SetActive(true);
        WinText.text = "World Wins!";
        
        // Move to next level or any transition logic
    }

    // Call this when "Me" dies
    public void OnPlayerDeath()
    {
        if (!hasWon) // Check if world hasn't already won
        {
            playerLives--;
            Debug.Log("Me lost a life. Remaining lives: " + playerLives);
            lifeText.text = "Lives: " + playerLives;
            if (playerLives <= 0)
            {
                WorldWins();  // Trigger WorldWins when lives are depleted
                return;  // Stop further logic
            }
        }
    }

    // You can call this function whenever the level transitions to update traps
    public void UpdateTrapCount()
    {
        //trapManager.SetTrapCount(trapCount); // Assume trap manager sets traps
        //Debug.Log("Traps updated to: " + trapCount);
    }

    // Detect when "Me" reaches the right wall (finish line)
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player collided with: " + other.gameObject.name); // To verify collision detection
        // Check if the object that collided has the "RightWall" tag
        if (other.gameObject.CompareTag(flagTag))
        {
            Debug.Log("Flag hit detected.");
            MeWins(); // Player reached the right wall (finish)
        }
    }

}

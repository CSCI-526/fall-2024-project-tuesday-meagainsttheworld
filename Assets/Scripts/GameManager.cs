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
    public TextMeshProUGUI scoreText;
    public Button nextButton;
    //public TextMeshProUGUI TrapsText;
    public TextMeshProUGUI WinText;
    public static int playerLives = 3;       // Starting lives for "Me"
    private bool hasGameReset = false;
    public bool isMeWinner = false;   // Track if "Me" wins
    public bool isWorldWinner = false; // Track if "World" wins
    private bool hasWon = false; //Track if "Me" has already won
    public bool isGameActive = false; // Track if the game has started
    public static int meScore { get; private set; } = 0;
    public static int worldScore { get; private set; } = 0;
    public string flagTag = "Flag";    // Tag for the right wall finish
    //private bool isNewGame = true; // Track if this is a new game
    private static bool isGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        playerLives = 3;

        // Reset scores at the beginning of the game
        // if (isNewGame)
        // {
        //     meScore = 0; // Reset Me's score
        //     worldScore = 0; // Reset World’s score
        //     isNewGame = false; // Set to false after the first game start
        // }

        // Reset scores if the game hasn't been started before
        if (!isGameStarted)
        {
            meScore = 0; 
            worldScore = 0; 
            isGameStarted = true; // Set to true after the first game start
        }

        if (lifeText == null)
        {
            lifeText = GameObject.Find("Life Text").GetComponent<TextMeshProUGUI>(); 
        }
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>(); 
        }
        if (nextButton == null)
        {
            nextButton = GameObject.Find("Next Button").GetComponent<Button>();  // Ensure you named the button correctly
        }
        // Hide the life and score text at the start
        lifeText.gameObject.SetActive(false); 
        scoreText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false); // Hide the next button initially
        if (!hasGameReset)
        {
            playerLives = 3;  // Reset to initial lives
            hasGameReset = true;  // Set the flag to prevent multiple resets
        }
        // Initialize lives and traps, if necessary
        Debug.Log("Game started. Player lives: " + playerLives);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameActive) return; // Don't update if the game hasn't started
        if (playerLives <= 0 && !hasWon)
        {
            WorldWins();
        }
    }

    public void StartGame(){
        titleScreen.gameObject.SetActive(false);
        isGameActive = true; // Set game to active
        // if (isNewGame)
        // {
        //     meScore = 0; // Reset Me's score
        //     worldScore = 0; // Reset World’s score
        //     isNewGame = false; // Set to false after the first game start
        // }
        // Show Lives and Score texts after pressing play
        lifeText.gameObject.SetActive(true); 
        scoreText.gameObject.SetActive(true);
        lifeText.text = "Lives: " + playerLives;
        //scoreText.text = $"Score: <color=blue>0</color> | <color=red>0</color>";
        UpdateScoreboardUI(); // Show current scores
        WinText.gameObject.SetActive(false); // Hide any Win text from previous runs
    }

    public void MeWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isMeWinner = true;
        //playerLives++; // Reward 1 extra life
        meScore++;
        Debug.Log("Me wins! Me's Score: "+ meScore);
        UpdateScoreboardUI();
        WinText.gameObject.SetActive(true);
        WinText.text = "Me Wins!";
        nextButton.gameObject.SetActive(true);
        
        // Move to next level or any transition logic
    }

    // Call this when "World" wins (e.g., player runs out of lives)
    public void WorldWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isWorldWinner = true;
        //trapCount++; // Reward an extra trap
        worldScore++;
        Debug.Log("World wins! World's Score: "+ worldScore);
        UpdateScoreboardUI();
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Destroy(playerController.gameObject);  // Destroy the player object to prevent further interaction
        }
        WinText.gameObject.SetActive(true);
        WinText.text = "World Wins!";
        nextButton.gameObject.SetActive(true);
        
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
    private void UpdateScoreboardUI()
    {
        scoreText.text = $"Score: <color=blue>{meScore}</color> | <color=red>{worldScore}</color>";
    }

    // Function to load the next level
    public void LoadNextLevel()
    {
        // SceneManager.LoadScene("Level 3"); // Replace "Level 2" with the actual scene name
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2"); // Load Level 2
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 3"); // Load Level 3
        }
        // Reset game state for new level if necessary
        hasWon = false; // Reset win status
        playerLives = 3; // Reset player lives for the new level
        lifeText.text = "Lives: " + playerLives; // Update life text
    }

    void OnApplicationQuit()
    {
        ResetStaticScores();
    }

    private void ResetStaticScores()
    {
        meScore = 0;
        worldScore = 0;
    }

}
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
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public GameObject trapTimer;
    public Button nextButton;
    public TextMeshProUGUI WinText;
    public GameObject winBackground;
    public PlayerHealth playerHealth;
    public int playerLives;
    private bool hasGameReset = false;
    public bool isMeWinner = false;   // Track if "Me" wins
    public bool isWorldWinner = false; // Track if "World" wins
    private bool hasWon = false; // Track if "Me" has already won
    public bool isGameActive = false; // Track if the game has started
    public static int meScore { get; private set; } = 0;
    public static int worldScore { get; private set; } = 0;
    public string flagTag = "Flag";    // Tag for the right wall finish
    private static bool isGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        playerLives = playerHealth.life;

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
        if (levelText == null)
        {
            levelText = GameObject.Find("Level Text").GetComponent<TextMeshProUGUI>(); 
        }
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>(); 
        }
        if (trapTimer == null)
        {
            trapTimer = GameObject.Find("TrapTimer"); 
        }
        if (nextButton == null)
        {
            nextButton = GameObject.Find("Next Button").GetComponent<Button>(); 
        }
        // Hide the life and score text and trap timer at the start
        lifeText.gameObject.SetActive(false); 
        levelText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        trapTimer.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false); // Hide the next button initially
        if (!hasGameReset)
        {
            playerLives = playerHealth.life;  // Reset to initial lives
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

    public void StartGame()
    {
        titleScreen.gameObject.SetActive(false);
        isGameActive = true; // Set game to active
        // Show Lives and Score texts and trap timer after pressing play
        lifeText.gameObject.SetActive(true); 
        levelText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        trapTimer.gameObject.SetActive(true);
        lifeText.text = "Lives: " + playerLives;
        UpdateScoreboardUI(); // Show current scores
        WinText.gameObject.SetActive(false); // Hide any Win text from previous runs
        winBackground.SetActive(false);
    }

    public void MeWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isMeWinner = true;
        meScore++;
        Debug.Log("Me wins! Me's Score: "+ meScore);
        UpdateScoreboardUI();
        WinText.gameObject.SetActive(true);
        WinText.text = "Me Wins!";
        winBackground.SetActive(true);
        WinText.rectTransform.anchoredPosition = new Vector2(120, WinText.rectTransform.anchoredPosition.y);
        // Disable next button if on Level 2
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            nextButton.gameObject.SetActive(false); // Hide the next button in Level 2
        }
        else
        {
            nextButton.gameObject.SetActive(true); // Show the next button for other levels
        }
    }

    public void WorldWins()
    {
        if (hasWon) return; // Prevent multiple calls
        hasWon = true; // Set the flag
        isWorldWinner = true;
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
        winBackground.SetActive(true);
        WinText.rectTransform.anchoredPosition = new Vector2(10, WinText.rectTransform.anchoredPosition.y);
        // Disable next button if on Level 2
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            nextButton.gameObject.SetActive(false); // Hide the next button in Level 3
        }
        else
        {
            nextButton.gameObject.SetActive(true); // Show the next button for other levels
        }
    }

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

    public void HandleFlag(Collider2D other)
    {
        // Additional logic...
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

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level 0-1")
        {
            SceneManager.LoadScene("Level 0-2"); // Load Level 0-2
        }
        else if (SceneManager.GetActiveScene().name == "Level 0-2")
        {
            SceneManager.LoadScene("Level 0-3");
        }
        else if (SceneManager.GetActiveScene().name == "Level 0-3")
        {
            ResetStaticScores();
            SceneManager.LoadScene("Level 1");
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }
        // Reset game state for new level if necessary
        hasWon = false; // Reset win status
        playerLives = playerHealth.life; // Reset player lives for the new level
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
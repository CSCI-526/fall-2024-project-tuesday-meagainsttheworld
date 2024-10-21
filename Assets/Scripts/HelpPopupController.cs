using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpPopupController : MonoBehaviour
{
    [SerializeField] private GameObject helpPopup;

    private bool isPopupVisible = false;

    public static bool isPaused = false;

    void Start()
    {
        // Ensure the popup is hidden initially
        if (helpPopup != null)
        {
            helpPopup.SetActive(false);
            Debug.Log("Help popup found and set to inactive.");
        }
        else
        {
            Debug.LogError("Help popup is not assigned.");
        }
    }

    void Update()
    {
        // Toggle the popup when the 'Tab' key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleHelpPopup();
        }
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) && isPopupVisible) // KeyCode.Return corresponds to the Enter key
        {
            ResumeGame();
            // Load the specified scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.M) && isPopupVisible) // KeyCode.M corresponds to the m/M key
        {
            ResumeGame();
            // Load the specified scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Method to handle button click
    public void OnHelpButtonClicked()
    {
        Debug.Log("Help button clicked.");
        ToggleHelpPopup();
    }

    // Method to handle close button click
    public void OnCloseButtonClicked()
    {
        Debug.Log("Close button clicked.");
        if (helpPopup != null)
            helpPopup.SetActive(false);

        isPopupVisible = false;
        ResumeGame();
    }

    // Toggle the visibility of the help popup
    private void ToggleHelpPopup()
    {
        isPopupVisible = !isPopupVisible;

        if (helpPopup != null)
            helpPopup.SetActive(isPopupVisible);

        // Pause or resume the game based on the popup visibility
        if (isPopupVisible)
            PauseGame();
        else
            ResumeGame();
    }

    // Pause the game
    private void PauseGame()
    {
        Time.timeScale = 0; // Stop time
        isPaused = true;
    }

    // Resume the game
    private void ResumeGame()
    {
        Time.timeScale = 1; // Resume time
        isPaused = false;
    }
}
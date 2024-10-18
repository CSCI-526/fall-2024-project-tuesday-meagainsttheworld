using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpPopupController : MonoBehaviour
{
    [SerializeField] private GameObject helpPopup;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button closeButton;

    private bool isPopupVisible = false;

    void Start()
    {
        // Ensure the popup is hidden initially
        if (helpPopup != null)
            helpPopup.SetActive(false);

        // Assign the OnHelpButtonClicked method to the button click event
        if (helpButton != null)
            helpButton.onClick.AddListener(OnHelpButtonClicked);

        // Assign the OnCloseButtonClicked method to the close button click event
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    void Update()
    {
        // Toggle the popup when the 'Tab' key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleHelpPopup();
        }
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return corresponds to the Enter key
        {
            ResumeGame();
            // Load the specified scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.M)) // KeyCode.Return corresponds to the Enter key
        {
            ResumeGame();
            // Load the specified scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Method to handle button click
    private void OnHelpButtonClicked()
    {
        ToggleHelpPopup();
    }

    // Method to handle close button click
    private void OnCloseButtonClicked()
    {
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
        Time.timeScale = 0f; // Stop time
    }

    // Resume the game
    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
    }
}
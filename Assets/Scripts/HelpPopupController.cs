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
        }
        else
        {
            Debug.LogError("Help popup is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleHelpPopup();
        }
        if (Input.GetKeyDown(KeyCode.Return) && isPopupVisible)
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.M) && isPopupVisible)
        {
            MainMenuSelect();
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

    public void MainMenuSelect()
    {
        ResumeGame();
        Debug.Log("Loading MainMenu");
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        ResumeGame();
        Debug.Log("Restarting current level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
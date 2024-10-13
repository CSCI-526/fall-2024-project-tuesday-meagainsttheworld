using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return corresponds to the Enter key
        {
            // Load the specified scene
            SceneManager.LoadScene("Level1");
        }

        if (Input.GetKeyDown(KeyCode.C)) // KeyCode.Return corresponds to the Enter key
        {
            // Load the specified scene
            SceneManager.LoadScene("ControlsMenu");
        }

        if (Input.GetKeyDown(KeyCode.M)) // KeyCode.Return corresponds to the Enter key
        {
            // Load the specified scene
            SceneManager.LoadScene("MainMenu");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return corresponds to the Enter key
        {
            // Load the specified scene
            SceneManager.LoadScene("SampleScene_K");
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

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static string lastLevel = "Level0";

    [SerializeField] private GameObject enterText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "YouWin")
        {
            if (lastLevel != "Level2") enterText.GetComponent<TextMeshProUGUI>().text = "Press 'ENTER' to go to next level";
            else enterText.GetComponent<TextMeshProUGUI>().text = "Press 'ENTER' to restart";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return corresponds to the Enter key
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "GameOver") SceneManager.LoadScene(lastLevel);
            else if (sceneName == "YouWin")
            {
                if (lastLevel == "Level0") SceneManager.LoadScene("Level1");
                else if (lastLevel == "Level1") SceneManager.LoadScene("Level2");
                else SceneManager.LoadScene("LevelsMenu");
            }
            else SceneManager.LoadScene("LevelsMenu");
        }

        if (Input.GetKeyDown(KeyCode.Tab)) // KeyCode.Return corresponds to the Enter key
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

    public void LevelSelect(string level)
    {
        Debug.Log(level);
        SceneManager.LoadScene("Level" + level);
    }
}

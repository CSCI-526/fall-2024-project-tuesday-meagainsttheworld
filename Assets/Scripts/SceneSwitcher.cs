using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static string prevLevel = "Level0";

    [SerializeField] private GameObject enterText;

    void Start()
    {
        if (string.IsNullOrEmpty(DataCollection.sessionID)) DataCollection.sessionID = DateTime.Now.Ticks.ToString();

        if (SceneManager.GetActiveScene().name == "YouWin")
        {
            if (prevLevel != "Level3") enterText.GetComponent<TextMeshProUGUI>().text = "Press 'ENTER' to go to next level";
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
            if (sceneName == "GameOver") SceneManager.LoadScene(prevLevel);
            else if (sceneName == "YouWin")
            {
                int lastLevelNum = int.Parse(prevLevel[5..]);
                if (lastLevelNum < 3) SceneManager.LoadScene(prevLevel[..5] + (lastLevelNum + 1).ToString());
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

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static string prevLevel = "Level0";

    private static readonly int finalLevelNum = 2;

    [SerializeField] private GameObject enterText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "YouWin")
        {
            bool validLvlName = int.TryParse(prevLevel[(prevLevel.LastIndexOf('l') + 1)..], out int lastLevelNum);
            if (validLvlName && lastLevelNum < finalLevelNum)
            {
                enterText.GetComponent<TextMeshProUGUI>().text = "Press 'ENTER' to go to next level";
                enterText.GetComponent<RectTransform>().anchoredPosition -= new Vector2(144, 0);
            }
        }
    }

    void Update()
    {
        if (Input.inputString != "" && SceneManager.GetActiveScene().name == "LevelsMenu")
        {
            bool isNum = int.TryParse(Input.inputString, out int lvl_num);

            if (isNum && lvl_num <= finalLevelNum) LevelSelect(lvl_num);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "YouWin")
            {
                bool validLvlName = int.TryParse(prevLevel[(prevLevel.LastIndexOf('l') + 1)..], out int lastLevelNum);
                if (validLvlName && lastLevelNum < finalLevelNum) LevelSelect(lastLevelNum + 1);
                else SceneManager.LoadScene("LevelsMenu");
            }
            else SceneManager.LoadScene("LevelsMenu");
        }

        if (Input.GetKeyDown(KeyCode.Tab)) SceneManager.LoadScene("ControlsMenu");

        if (Input.GetKeyDown(KeyCode.M)) SceneManager.LoadScene("MainMenu");
    }

    public void LevelSelect(int level)
    {
        Debug.Log("Loading level " + level);
        SceneManager.LoadScene("Level" + level);
    }
}

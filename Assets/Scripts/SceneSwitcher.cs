using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public static string prevLevel = "Level0";

    private static readonly int finalLevelNum = 2;

    void Start()
    {
        SpawnpointManager.playerSpawnStates.Clear();
        SpawnpointManager.lastCheckpointNum = 0;
        CameraManager.currCam = 1;
        if (SceneManager.GetActiveScene().name == "YouWin")
        {
            bool validLvlName = int.TryParse(prevLevel[(prevLevel.LastIndexOf('l') + 1)..], out int lastLevelNum);
            GameObject continueBtn = GameObject.Find("ContinueButton");
            if (validLvlName && lastLevelNum < finalLevelNum)
            {
                continueBtn.GetComponent<Button>().onClick.AddListener(delegate {SceneSelect("Level" + (lastLevelNum + 1));});
            }
            else
            {
                continueBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Restart";
                continueBtn.GetComponent<Button>().onClick.AddListener(delegate {SceneSelect("LevelsMenu");});
            }
        }
    }

    void Update()
    {
        if (Input.inputString != "" && SceneManager.GetActiveScene().name == "LevelsMenu")
        {
            bool isNum = int.TryParse(Input.inputString, out int lvl_num);

            if (isNum && lvl_num <= finalLevelNum) SceneSelect("Level" + lvl_num);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "YouWin")
            {
                bool validLvlName = int.TryParse(prevLevel[(prevLevel.LastIndexOf('l') + 1)..], out int lastLevelNum);
                if (validLvlName && lastLevelNum < finalLevelNum) SceneSelect("Level" + (lastLevelNum + 1));
                else SceneManager.LoadScene("LevelsMenu");
            }
            else SceneManager.LoadScene("LevelsMenu");
        }

        if (Input.GetKeyDown(KeyCode.Tab)) SceneManager.LoadScene("ControlsMenu");

        if (Input.GetKeyDown(KeyCode.M)) SceneManager.LoadScene("MainMenu");
    }

    public void SceneSelect(string sceneName)
    {
        Debug.Log("Loading " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
